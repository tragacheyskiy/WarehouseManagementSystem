using WarehouseManagementSystem.Domain.Dtos;

namespace WarehouseManagementSystem.Domain.Models;

public sealed class ProductsTransfer : IEquatable<ProductsTransfer>
{
    public enum Type
    {
        Receive,
        Dispatch
    }

    public const Type Receive = Type.Receive;
    public const Type Dispatch = Type.Dispatch;

    public ProductsTransfer(long time, Guid sourceWarehouseId, Guid targetWarehouseId, IReadOnlyList<QuantitativeProductDto> products)
    {
        Time = time;
        SourceWarehouseId = sourceWarehouseId;
        TargetWarehouseId = targetWarehouseId;
        Products = products;
    }

    public Guid SourceWarehouseId { get; }
    public Guid TargetWarehouseId { get; }
    public IReadOnlyList<QuantitativeProductDto> Products { get; }
    public long Time { get; }

    public bool Equals(ProductsTransfer? other)
    {
        return other is not null
            && SourceWarehouseId == other.SourceWarehouseId
            && TargetWarehouseId == other.TargetWarehouseId
            && Products == other.Products
            && Time == other.Time;
    }
}
