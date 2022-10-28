using WarehouseManagementSystem.Data.Entities;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Data.Mappers;

internal static class ProductStockMapper
{
    public static List<QuantitativeProduct> Map(List<ProductStock> entities)
    {
        return entities.Select(x => Map(x)).ToList();
    }

    public static QuantitativeProduct Map(ProductStock entity)
    {
        return new QuantitativeProduct(entity.ProductId, entity.Quantity);
    }
}
