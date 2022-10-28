using WarehouseManagementSystem.Data.Entities;
using WarehouseManagementSystem.Domain.Dtos;

namespace WarehouseManagementSystem.Data.Mappers;

internal static class TransferProductStockMapper
{
    public static List<QuantitativeProductDto> Map(List<TransferProductStock> entities)
    {
        return entities.Select(x => Map(x)).ToList();
    }

    public static QuantitativeProductDto Map(TransferProductStock entity)
    {
        return new QuantitativeProductDto(entity.ProductId, entity.Quantity);
    }
}
