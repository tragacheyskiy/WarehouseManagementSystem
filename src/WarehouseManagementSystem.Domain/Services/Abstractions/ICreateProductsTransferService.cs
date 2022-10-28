using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Domain.Services.Abstractions;

public interface ICreateProductsTransferService
{
    Task CreateAsync(ProductsTransfer productTransfer);
}
