using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Domain.Services.Abstractions;

public interface IGetProductsTransferService
{
    Task<IList<ProductsTransfer>> GetAsync(Guid warehouseId, long minTime, ProductsTransfer.Type type);
}
