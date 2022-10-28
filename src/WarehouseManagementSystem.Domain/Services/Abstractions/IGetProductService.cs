using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Domain.Services.Abstractions;

public interface IGetProductService
{
    Task<IList<Product>> GetAsync();
}
