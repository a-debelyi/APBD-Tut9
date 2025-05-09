namespace APBD_9.Repositories;

public interface IWarehousesRepository
{
    Task<bool> WarehouseExistsAsync(int idWarehouse);
}