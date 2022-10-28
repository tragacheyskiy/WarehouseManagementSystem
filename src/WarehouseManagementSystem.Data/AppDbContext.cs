using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using WarehouseManagementSystem.Data.Entities;

namespace WarehouseManagementSystem.Data;

#nullable disable

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        bool exists = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator)!.Exists();

        if (!exists)
        {
            Database.EnsureCreated();

            Guid bookId = Products.Add(new Product { Name = "Book" }).Entity.Id;
            Guid phoneId = Products.Add(new Product { Name = "Phone" }).Entity.Id;
            Guid chairId = Products.Add(new Product { Name = "Chair" }).Entity.Id;
            Guid mirrorId = Products.Add(new Product { Name = "Mirror" }).Entity.Id;
            Guid brushId = Products.Add(new Product { Name = "Brush" }).Entity.Id;
            Guid screwdriverId = Products.Add(new Product { Name = "Screwdriver" }).Entity.Id;
            Guid charcoalId = Products.Add(new Product { Name = "Charcoal" }).Entity.Id;

            Warehouses.Add(new Warehouse
            {
                Name = "Small Warehouse",
                Products = new List<ProductStock>
                {
                    new ProductStock { ProductId = bookId, Quantity = 5 },
                    new ProductStock { ProductId = phoneId, Quantity = 5 },
                }
            });
            Warehouses.Add(new Warehouse
            {
                Name = "Medium Warehouse",
                Products = new List<ProductStock>
                {
                    new ProductStock { ProductId = bookId, Quantity = 5 },
                    new ProductStock { ProductId = phoneId, Quantity = 5 },
                    new ProductStock { ProductId = chairId, Quantity = 5 },
                    new ProductStock { ProductId = mirrorId, Quantity = 5 },

                }
            });
            Warehouses.Add(new Warehouse
            {
                Name = "Large Warehouse",
                Products = new List<ProductStock>
                {
                    new ProductStock { ProductId = chairId, Quantity = 5 },
                    new ProductStock { ProductId = charcoalId, Quantity = 5 },
                    new ProductStock { ProductId = brushId, Quantity = 5 },
                    new ProductStock { ProductId = screwdriverId, Quantity = 5 },
                    new ProductStock { ProductId = mirrorId, Quantity = 5 },
                    new ProductStock { ProductId = bookId, Quantity = 5 },
                }
            });

            SaveChanges();
        }
    }

    internal DbSet<Warehouse> Warehouses { get; set; }
    internal DbSet<Product> Products { get; set; }
    internal DbSet<ProductsTransfer> ProductsTransfers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (IReadOnlyEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType is not null && entityType.ClrType.IsAssignableTo(typeof(Entity)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(Entity.CreatedAt))
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}
