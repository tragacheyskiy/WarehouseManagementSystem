using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Data.Mappers;

internal static class ProductsTransferMapper
{
    public static List<ProductsTransfer> Map(List<Entities.ProductsTransfer> entities)
    {
        return entities.Select(x => Map(x)).ToList();
    }

    public static ProductsTransfer Map(Entities.ProductsTransfer entity)
    {
        return new ProductsTransfer(entity.Time, entity.SourceWarehouseId, entity.TargetWarehouseId,
            TransferProductStockMapper.Map(entity.Products));
    }
}
