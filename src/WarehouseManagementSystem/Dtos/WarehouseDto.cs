using WarehouseManagementSystem.Domain.Dtos;

namespace WarehouseManagementSystem.Dtos;

public sealed class WarehouseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public IEnumerable<QuantitativeProductDto> Products { get; set; } = default!;
}
