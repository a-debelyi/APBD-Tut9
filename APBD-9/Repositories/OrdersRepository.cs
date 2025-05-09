using APBD_9.Models;
using Microsoft.Data.SqlClient;

namespace APBD_9.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly string _connectionString;

    public OrdersRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<Order?> GetMatchingOrderAsync(int idProduct, int amount, DateTime createdAt)
    {
        string sql = @"
                SELECT TOP 1 idOrder, createdAt
                FROM [order]
                WHERE idProduct = @idProduct AND amount = @amount AND createdAt < @createdAt
        ";

        await using SqlConnection conn = new(_connectionString);
        await using (SqlCommand comm = new(sql, conn))
        {
            await conn.OpenAsync();
            comm.Parameters.AddWithValue("@idProduct", idProduct);
            comm.Parameters.AddWithValue("@amount", amount);
            comm.Parameters.AddWithValue("@createdAt", createdAt);
            await using (SqlDataReader reader = await comm.ExecuteReaderAsync())
            {
                if (!await reader.ReadAsync()) return null;
                return new Order()
                {
                    IdOrder = reader.GetInt32(0),
                    IdProduct = idProduct,
                    CreatedAt = reader.GetDateTime(1),
                    Amount = amount
                };
            }
        }
    }
}