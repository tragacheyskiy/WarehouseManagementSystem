using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Services.Abstractions;

namespace WarehouseManagementSystem.Data.Repositories;

public sealed class ProductRepository : IGetProductService
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IList<Product>> GetAsync()
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Select(x => new Product(x.Id, x.Name))
            .ToListAsync();
    }
}
