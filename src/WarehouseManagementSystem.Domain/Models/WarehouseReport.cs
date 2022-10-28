using WarehouseManagementSystem.Domain.Dtos;

namespace WarehouseManagementSystem.Domain.Models;

public class WarehouseReport : IEquatable<WarehouseReport>
{
    public WarehouseReport(long time, string warehouseName, IReadOnlyList<QuantitativeProductDto> products)
    {
        Time = time;
        WarehouseName = warehouseName ?? throw new ArgumentNullException(nameof(warehouseName));
        Products = products ?? throw new ArgumentNullException(nameof(products));
    }

    public long Time { get; }
    public string WarehouseName { get; }
    public IReadOnlyList<QuantitativeProductDto> Products { get; }

    public bool Equals(WarehouseReport? other)
    {
        return other is not null
            && Time == other.Time
            && WarehouseName == other.WarehouseName
            && Products.SequenceEqual(other.Products);
    }
}
