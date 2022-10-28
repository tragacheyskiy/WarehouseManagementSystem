using WarehouseManagementSystem.Domain.Dtos;

namespace WarehouseManagementSystem.Dtos;

public sealed class ProductsTransferRequestDto
{
    public Guid SourceWarehouseId { get; set; }
    public Guid TargetWarehouseId { get; set; }
    public List<QuantitativeProductDto> Products { get; set; } = default!;
}
