using WarehouseManagementSystem.Domain.Dtos;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Domain.Tests;

public sealed class WarehouseTests
{
    [Fact]
    public void Successful_products_receiving()
    {
        Guid productId = Guid.NewGuid();
        var products = new List<QuantitativeProduct> { new QuantitativeProduct(Guid.Empty, productId, 7) };
        var transferProducts = new List<QuantitativeProductDto> { new QuantitativeProductDto(productId, 10) };
        var sut = new Warehouse(Guid.NewGuid(), string.Empty, products);

        bool result = sut.ReceiveProducts(transferProducts);

        Assert.True(result);
        Assert.Equal(expected: 17, actual: sut.Products[0].Quantity);
    }

    [Fact]
    public void Successful_new_products_receiving()
    {
        var products = Enumerable.Empty<QuantitativeProduct>().ToList();
        var transferProducts = new List<QuantitativeProductDto> { new QuantitativeProductDto(Guid.NewGuid(), 10) };
        var sut = new Warehouse(Guid.NewGuid(), string.Empty, products);

        bool result = sut.ReceiveProducts(transferProducts);

        Assert.True(result);
        Assert.Equal(transferProducts[0].ProductId, sut.Products[0].ProductId);
        Assert.Equal(transferProducts[0].Quantity, sut.Products[0].Quantity);
    }

    [Fact]
    public void Receiving_products_with_incorrect_quantity_is_fails()
    {
        var products = Enumerable.Empty<QuantitativeProduct>().ToList();
        var transferProducts = new List<QuantitativeProductDto> { new QuantitativeProductDto(Guid.NewGuid(), 0) };
        var sut = new Warehouse(Guid.NewGuid(), string.Empty, products);

        bool result = sut.ReceiveProducts(transferProducts);

        Assert.False(result);
        Assert.Equal(expected: 0, actual: sut.Products.Count);
    }

    [Fact]
    public void Dispatching_products_with_quantity_more_than_stock_is_fails()
    {
        Guid productId = Guid.NewGuid();
        var products = new List<QuantitativeProduct> { new QuantitativeProduct(Guid.Empty, productId, 10) };
        var transferProducts = new List<QuantitativeProductDto> { new QuantitativeProductDto(Guid.NewGuid(), 15) };
        var sut = new Warehouse(Guid.NewGuid(), string.Empty, products);

        bool result = sut.DispatchProducts(transferProducts);

        Assert.False(result);
        Assert.Equal(expected: 10, actual: sut.Products[0].Quantity);
    }

    [Fact]
    public void Dispatching_non_existent_products_is_fails()
    {
        Guid productId = Guid.NewGuid();
        var products = new List<QuantitativeProduct> { new QuantitativeProduct(Guid.Empty, productId, 10) };
        var transferProducts = new List<QuantitativeProductDto> { new QuantitativeProductDto(Guid.NewGuid(), 5) };
        var sut = new Warehouse(Guid.NewGuid(), string.Empty, products);

        bool result = sut.DispatchProducts(transferProducts);

        Assert.False(result);
        Assert.Equal(expected: 10, actual: sut.Products[0].Quantity);
    }    

    [Fact]
    public void Successful_products_dispathing()
    {
        Guid productId = Guid.NewGuid();
        var products = new List<QuantitativeProduct> { new QuantitativeProduct(Guid.Empty, productId, 10) };
        var transferProducts = new List<QuantitativeProductDto> { new QuantitativeProductDto(productId, 5) };
        var sut = new Warehouse(Guid.NewGuid(), string.Empty, products);

        bool result = sut.DispatchProducts(transferProducts);

        Assert.True(result);
        Assert.Equal(expected: 5, actual: sut.Products[0].Quantity);
    }
}
