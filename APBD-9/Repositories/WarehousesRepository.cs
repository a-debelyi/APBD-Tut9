using Microsoft.Data.SqlClient;

namespace APBD_9.Repositories;

public class WarehousesRepository : IWarehousesRepository
{
    private readonly string _connectionString;

    public WarehousesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> WarehouseExistsAsync(int idWarehouse)
    {
        string sql = "SELECT 1 FROM warehouse WHERE idWarehouse = @idWarehouse";

        await using (SqlConnection conn = new(_connectionString))
        await using (SqlCommand comm = new(sql, conn))
        {
            await conn.OpenAsync();
            comm.Parameters.AddWithValue("@idWarehouse", idWarehouse);
            return null != await comm.ExecuteScalarAsync();
        }
    }
}