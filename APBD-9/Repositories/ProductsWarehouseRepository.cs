using System.Data;
using APBD_8.Exceptions;
using APBD_9.DTOs;
using Microsoft.Data.SqlClient;

namespace APBD_9.Repositories;

public class ProductsWarehouseRepository : IProductsWarehouseRepository
{
    private readonly string _connectionString;

    public ProductsWarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> ProductWarehouseExistsAsync(int idOrder)
    {
        string sql = "SELECT 1 FROM product_warehouse WHERE idOrder = @idOrder";

        await using (SqlConnection conn = new(_connectionString))
        await using (SqlCommand command = new(sql, conn))
        {
            await conn.OpenAsync();
            command.Parameters.AddWithValue("@idOrder", idOrder);
            return null != await command.ExecuteScalarAsync();
        }
    }

    public async Task<int> FulfillOrderAsync(ProductWarehouseDto productWarehouseDto, int idOrder)
    {
        await using (SqlConnection conn = new(_connectionString))
        {
            await conn.OpenAsync();
            SqlTransaction transaction = conn.BeginTransaction();
            try
            {
                var now = DateTime.UtcNow;

                string updateOrderSql = "UPDATE [order] SET fulfilledAt = @now WHERE idOrder = @idOrder";
                await using (SqlCommand comm = new(updateOrderSql, conn, transaction))
                {
                    comm.Parameters.AddWithValue("@idOrder", idOrder);
                    comm.Parameters.AddWithValue("@now", now);
                    await comm.ExecuteNonQueryAsync();
                }

                decimal price;
                string getPriceSql = "SELECT price FROM product WHERE idProduct = @idProduct";
                await using (SqlCommand comm = new(getPriceSql, conn, transaction))
                {
                    comm.Parameters.AddWithValue("@idProduct", productWarehouseDto.IdProduct);
                    using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        price = reader.GetDecimal(0);
                    }
                }

                int newId;
                string insertProductWarehouseSql = @"
                                INSERT INTO product_warehouse (idWarehouse, idProduct, idOrder, amount, price, createdAt)
                                VALUES (@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAt)
                                SELECT CAST(SCOPE_IDENTITY() AS int);
                    ";
                await using (SqlCommand comm = new(insertProductWarehouseSql, conn, transaction))
                {
                    comm.Parameters.AddWithValue("@idWarehouse", productWarehouseDto.IdWarehouse);
                    comm.Parameters.AddWithValue("@idProduct", productWarehouseDto.IdProduct);
                    comm.Parameters.AddWithValue("@idOrder", idOrder);
                    comm.Parameters.AddWithValue("@amount", productWarehouseDto.Amount);
                    comm.Parameters.AddWithValue("@price", productWarehouseDto.Amount * price);
                    comm.Parameters.AddWithValue("@createdAt", now);
                    newId = Convert.ToInt32(await comm.ExecuteScalarAsync());
                }

                transaction.Commit();
                return newId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public async Task<int> FulfillOrderViaProcedureAsync(ProductWarehouseDto productWarehouseDto)
    {
        await using (SqlConnection conn = new(_connectionString))
        await using (SqlCommand comm = new("AddProductToWarehouse", conn))
        {
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.AddWithValue("@IdWarehouse", productWarehouseDto.IdWarehouse);
            comm.Parameters.AddWithValue("@IdProduct", productWarehouseDto.IdProduct);
            comm.Parameters.AddWithValue("@Amount", productWarehouseDto.Amount);
            comm.Parameters.AddWithValue("@CreatedAt", productWarehouseDto.CreatedAt);

            await conn.OpenAsync();
            try
            {
                var newId = await comm.ExecuteScalarAsync();
                return Convert.ToInt32(newId);
            }
            catch (SqlException e)
            {
                var err = e.Errors.Cast<SqlError>().First();
                switch (err.State)
                {
                    case 1: throw new NotFoundException(err.Message);
                    case 2: throw new NotFoundException(err.Message);
                    case 3: throw new ConflictException(err.Message);
                    default: throw;
                }
            }
        }
    }
}