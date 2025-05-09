using APBD_9.DTOs;

namespace APBD_9.Services;

public interface IProductsWarehouseService
{
    Task<int> AddProductToWarehouseAsync(ProductWarehouseDto productWarehouseDto);
}