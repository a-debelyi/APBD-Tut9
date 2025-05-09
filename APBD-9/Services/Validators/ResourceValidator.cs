using APBD_8.Exceptions;
using APBD_9.Repositories;

namespace APBD_9.Services.Validators;

public class ResourceValidator : IResourceValidator
{
    private readonly IProductsRepository _productsRepository;
    private readonly IWarehousesRepository _warehousesRepository;

    public ResourceValidator(IProductsRepository productsRepository, IWarehousesRepository warehousesRepository)
    {
        _productsRepository = productsRepository;
        _warehousesRepository = warehousesRepository;
    }

    public async Task EnsureProductExists(int idProduct)
    {
        if (!await _productsRepository.ProductExistsAsync(idProduct))
            throw new NotFoundException($"Product with ID {idProduct} does not exist");
    }

    public async Task EnsureWarehouseExists(int idWarehouse)
    {
        if (!await _warehousesRepository.WarehouseExistsAsync(idWarehouse))
            throw new NotFoundException($"Warehouse with ID {idWarehouse} does not exist");
    }
}