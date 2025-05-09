using APBD_8.Exceptions;
using APBD_9.Models;
using APBD_9.Repositories;

namespace APBD_9.Services.Finders;

public class OrderFinder : IOrderFinder
{
    private readonly IOrdersRepository _ordersRepository;

    public OrderFinder(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<Order> FindAsync(int idProduct, int amount, DateTime createdAt)
    {
        return await _ordersRepository.GetMatchingOrderAsync(idProduct, amount, createdAt)
               ?? throw new NotFoundException(
                   $"No matching order found for product with ID {idProduct} with amount {amount} before {createdAt}");
    }
}