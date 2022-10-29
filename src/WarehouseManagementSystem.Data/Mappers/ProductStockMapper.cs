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
        return new QuantitativeProduct(entity.Id, entity.ProductId, entity.Quantity);
    }

    public static List<ProductStock> Map(IReadOnlyList<QuantitativeProduct> models)
    {
        return models.Select(x => Map(x)).ToList();
    }

    public static ProductStock Map(QuantitativeProduct model)
    {
        return new ProductStock
        {
            Id = model.Id,
            ProductId = model.ProductId,
            Quantity = model.Quantity
        };
    }
}
