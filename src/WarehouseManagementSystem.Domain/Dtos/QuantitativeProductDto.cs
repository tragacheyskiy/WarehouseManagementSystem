namespace WarehouseManagementSystem.Domain.Dtos;

public sealed class QuantitativeProductDto : IEquatable<QuantitativeProductDto>
{
    public QuantitativeProductDto(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public Guid ProductId { get; }
    public int Quantity { get; }

    public bool Equals(QuantitativeProductDto? other)
    {
        return other is not null
            && ProductId == other.ProductId
            && Quantity == other.Quantity;
    }
}
