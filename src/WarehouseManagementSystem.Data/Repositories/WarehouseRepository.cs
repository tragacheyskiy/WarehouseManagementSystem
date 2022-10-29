using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using WarehouseManagementSystem.Data.Mappers;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Services.Abstractions;

namespace WarehouseManagementSystem.Data.Repositories;

public sealed class WarehouseRepository : IGetWarehouseByIdService, IGetWarehouseService, IUpdateWarehouseService
{
    private readonly AppDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public WarehouseRepository(AppDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public async Task<IList<Warehouse>> GetAsync()
    {
        var result = await _dbContext.Warehouses
            .AsNoTracking()
            .Include(x => x.Products)
            .Where(x => x.DeletedAt == null)
            .ToListAsync();

        return WarehouseMapper.Map(result);
    }

    public async Task<Warehouse?> GetAsync(Guid id)
    {
        var result = await _dbContext.Warehouses
            .AsNoTracking()
            .Include(x => x.Products)
            .Where(x => x.DeletedAt == null)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (result is null)
            return null;

        return WarehouseMapper.Map(result);
    }

    public Task UpdateAsync(Warehouse warehouse)
    {
        var now = _dateTimeProvider.NowUtc;

        var entity = WarehouseMapper.Map(warehouse);
        entity.ModifiedAt = now;

        foreach(var product in entity.Products)
        {
            if (product.Id == Guid.Empty)
            {
                product.CreatedAt = now;
            }
            else
            {
                product.ModifiedAt = now;
            }
        }

        _dbContext.Warehouses.Update(entity);

        return _dbContext.SaveChangesAsync();
    }
}
