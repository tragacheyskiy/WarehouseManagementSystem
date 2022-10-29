using WarehouseManagementSystem.Domain.Dtos;

namespace WarehouseManagementSystem.Domain.Models;

public sealed class Warehouse
{
    private readonly List<QuantitativeProduct> _products;

    public Warehouse(Guid id, string name, List<QuantitativeProduct> products)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _products = products ?? throw new ArgumentNullException(nameof(products));
    }

    public Guid Id { get; }
    public string Name { get; }
    public IReadOnlyList<QuantitativeProduct> Products => _products;

    public bool ReceiveProducts(IReadOnlyList<QuantitativeProductDto> products)
    {
        foreach (var quantitativeProductDto in products)
        {
            if (quantitativeProductDto.Quantity <= 0)
                return false;

            QuantitativeProduct? existingProduct = _products.SingleOrDefault(x => x.ProductId == quantitativeProductDto.ProductId);

            if (existingProduct is null)
            {
                var newProduct = new QuantitativeProduct(Guid.Empty, quantitativeProductDto.ProductId, quantitativeProductDto.Quantity);
                _products.Add(newProduct);
                continue;
            }

            if (!existingProduct.IncreaseQuantity(quantitativeProductDto.Quantity))
                return false;
        }

        return true;
    }

    public bool DispatchProducts(IReadOnlyList<QuantitativeProductDto> products)
    {
        foreach (var quantitativeProductDto in products)
        {
            QuantitativeProduct? existingProduct = _products.SingleOrDefault(x => x.ProductId == quantitativeProductDto.ProductId);

            if (existingProduct is null || !existingProduct.DecreaseQuantity(quantitativeProductDto.Quantity))
                return false;
        }

        return true;
    }
}
