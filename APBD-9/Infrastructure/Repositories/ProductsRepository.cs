using Microsoft.Data.SqlClient;

namespace APBD_9.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly string _connectionString;

    public ProductsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> ProductExistsAsync(int idProduct)
    {
        string sql = "SELECT 1 FROM product WHERE idProduct = @idProduct";
        await using (SqlConnection conn = new(_connectionString))
        await using (SqlCommand comm = new(sql, conn))
        {
            await conn.OpenAsync();
            comm.Parameters.AddWithValue("@idProduct", idProduct);
            return null != await comm.ExecuteScalarAsync();
        }
    }
}