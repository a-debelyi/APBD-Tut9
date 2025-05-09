using APBD_9.DTOs;

namespace APBD_9.Repositories;

public interface IProductsWarehouseRepository
{
    Task<bool> ProductWarehouseExistsAsync(int idOrder);
    Task<int> FulfillOrderAsync(ProductWarehouseDto productWarehouseDto, int idOrder);
}