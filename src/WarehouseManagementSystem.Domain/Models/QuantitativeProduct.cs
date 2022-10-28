namespace WarehouseManagementSystem.Domain.Models;

public sealed class QuantitativeProduct
{
    public QuantitativeProduct(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public Guid ProductId { get; }
    public int Quantity { get; private set; }

    public bool IncreaseQuantity(int value)
    {
        if (value <= 0) return false;

        Quantity += value;

        return true;
    }

    public bool DecreaseQuantity(int value)
    {
        if (value <= 0 || value > Quantity) return false;

        Quantity -= value;

        return true;
    }
}
