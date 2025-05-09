using APBD_9.Models;

namespace APBD_9.Services.Finders;

public interface IOrderFinder
{
    Task<Order> FindAsync(int idProduct, int amount, DateTime createdAt);
}