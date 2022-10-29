using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data.Mappers;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Services.Abstractions;

namespace WarehouseManagementSystem.Data.Repositories;

public sealed class ProductsTransferRepository : ICreateProductsTransferService, IGetProductsTransferService
{
    private readonly AppDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProductsTransferRepository(AppDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public Task CreateAsync(ProductsTransfer productTransfer)
    {
        var entity = new Entities.ProductsTransfer
        {
            CreatedAt = _dateTimeProvider.NowUtc,
            Time = productTransfer.Time,
            SourceWarehouseId = productTransfer.SourceWarehouseId,
            TargetWarehouseId = productTransfer.TargetWarehouseId,
            Products = productTransfer.Products
            .Select(x => new Entities.TransferProductStock { ProductId = x.ProductId, Quantity = x.Quantity })
            .ToList()
        };

        _dbContext.ProductsTransfers.Add(entity);

        return _dbContext.SaveChangesAsync();
    }

    public async Task<IList<ProductsTransfer>> GetAsync(Guid warehouseId, long minTime, ProductsTransfer.Type type)
    {
        var query = _dbContext.ProductsTransfers
            .AsNoTracking()
            .Include(x => x.Products)
            .AsSplitQuery()
            .Where(x => x.DeletedAt == null);

        switch (type)
        {
            case ProductsTransfer.Receive:
                query = query.Where(x => x.TargetWarehouseId == warehouseId);
                break;
            case ProductsTransfer.Dispatch:
                query = query.Where(x => x.SourceWarehouseId == warehouseId);
                break;
        }

        var result = await query.ToListAsync();
        return ProductsTransferMapper.Map(result);
    }
}
