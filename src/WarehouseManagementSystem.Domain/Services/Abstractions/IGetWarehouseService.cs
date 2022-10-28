using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Domain.Services.Abstractions;

public interface IGetWarehouseService
{
    Task<IList<Warehouse>> GetAsync();
}
