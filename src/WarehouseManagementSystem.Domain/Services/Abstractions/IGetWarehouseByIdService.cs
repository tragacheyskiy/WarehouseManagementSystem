using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Domain.Services.Abstractions;

public interface IGetWarehouseByIdService
{
    Task<Warehouse?> GetAsync(Guid id);
}
