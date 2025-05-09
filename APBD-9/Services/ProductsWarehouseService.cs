using APBD_8.Exceptions;
using APBD_9.DTOs;
using APBD_9.Models;
using APBD_9.Repositories;
using APBD_9.Services.Finders;
using APBD_9.Services.Validators;

namespace APBD_9.Services;

public class ProductsWarehouseService : IProductsWarehouseService
{
    private readonly IProductsWarehouseRepository _productsWarehouseRepository;
    private readonly IResourceValidator _resourceValidator;
    private readonly IOrderFinder _orderFinder;

    public ProductsWarehouseService(IProductsWarehouseRepository productsWarehouseRepository,
        IResourceValidator resourceValidator, IOrderFinder orderFinder)
    {
        _productsWarehouseRepository = productsWarehouseRepository;
        _resourceValidator = resourceValidator;
        _orderFinder = orderFinder;
    }

    public async Task<int> AddProductToWarehouseAsync(ProductWarehouseDto productWarehouseDto)
    {
        await _resourceValidator.EnsureProductExists(productWarehouseDto.IdProduct);
        await _resourceValidator.EnsureWarehouseExists(productWarehouseDto.IdWarehouse);

        var order = await _orderFinder.FindAsync(productWarehouseDto.IdProduct, productWarehouseDto.Amount,
            productWarehouseDto.CreatedAt);

        if (await _productsWarehouseRepository.ProductWarehouseExistsAsync(order.IdOrder))
            throw new ConflictException($"Product warehouse for order {order.IdOrder} already exists");

        return await _productsWarehouseRepository.FulfillOrderAsync(productWarehouseDto, order.IdOrder);
    }
}