using APBD_9.Models;

namespace APBD_9.Services.Validators;

public interface IResourceValidator
{
    Task EnsureProductExists(int idProduct);
    Task EnsureWarehouseExists(int idWarehouse);
}