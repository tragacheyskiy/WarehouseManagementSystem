using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Domain.Services.Abstractions;

public interface IUpdateWarehouseService
{
    Task UpdateAsync(Warehouse warehouse);
}
