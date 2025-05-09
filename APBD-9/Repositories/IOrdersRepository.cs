using APBD_9.Models;

namespace APBD_9.Repositories;

public interface IOrdersRepository
{
    Task<Order?> GetMatchingOrderAsync(int idProduct, int amount, DateTime createdAt);
}