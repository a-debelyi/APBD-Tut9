namespace APBD_9.Repositories;

public interface IProductsRepository
{
    Task<bool> ProductExistsAsync(int idProduct);
}