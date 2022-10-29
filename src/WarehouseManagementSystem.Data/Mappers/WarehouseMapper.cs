using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Data.Mappers;

internal static class WarehouseMapper
{
    public static List<Warehouse> Map(List<Entities.Warehouse> entities)
    {
        return entities.Select(x => Map(x)).ToList();
    }

    public static Warehouse Map(Entities.Warehouse entity)
    {
        return new Warehouse(entity.Id, entity.Name, ProductStockMapper.Map(entity.Products));
    }

    public static Entities.Warehouse Map(Warehouse model)
    {
        return new Entities.Warehouse
        {
            Id = model.Id,
            Name = model.Name,
            Products = ProductStockMapper.Map(model.Products)
        };
    }
}
