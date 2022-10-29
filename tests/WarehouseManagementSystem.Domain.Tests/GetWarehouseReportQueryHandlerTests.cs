using Moq;
using WarehouseManagementSystem.Domain.Dtos;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.Domain.QueryHandlers;
using WarehouseManagementSystem.Domain.Services.Abstractions;

namespace WarehouseManagementSystem.Domain.Tests;

public sealed class GetWarehouseReportQueryHandlerTests
{
    [Fact]
    public async void If_there_no_transfers_report_returns_warehouse_current_state()
    {
        // Arrange
        Guid warehouseId = Guid.NewGuid();
        Guid productId = Guid.NewGuid();
        long time = 2022_10_28_00_00_00;

        var products = new List<QuantitativeProduct> { new QuantitativeProduct(Guid.Empty, productId, 10) };
        Warehouse warehouse = new(warehouseId, "Source Warehouse", products);

        var getWarehouseServiceStub = new Mock<IGetWarehouseByIdService>();
        getWarehouseServiceStub
            .Setup(x => x.GetAsync(warehouseId))
            .Returns(Task.FromResult(warehouse)!);

        var getProductsTransferServiceStub = new Mock<IGetProductsTransferService>();

        getProductsTransferServiceStub
            .Setup(x => x.GetAsync(warehouseId, time, ProductsTransfer.Receive))
            .Returns(Task.FromResult<IList<ProductsTransfer>>(Enumerable.Empty<ProductsTransfer>().ToList()));

        getProductsTransferServiceStub
            .Setup(x => x.GetAsync(warehouseId, time, ProductsTransfer.Dispatch))
            .Returns(Task.FromResult<IList<ProductsTransfer>>(Enumerable.Empty<ProductsTransfer>().ToList()));

        var sut = new GetWarehouseReportQueryHandler(getWarehouseServiceStub.Object, getProductsTransferServiceStub.Object);

        // Act
        WarehouseReport? result = await sut.HandleAsync(new GetWarehouseReportQuery(warehouseId, time));

        // Assert
        var expected = new WarehouseReport(time, warehouse.Name, new[] { new QuantitativeProductDto(productId, 10) });
        Assert.True(expected.Equals(result));
    }

    [Fact]
    public async void Successful_report()
    {
        // Arrange
        Guid warehouseId = Guid.NewGuid();
        Guid productId = Guid.NewGuid();
        long time = 2022_10_28_00_00_00;

        var products = new List<QuantitativeProduct> { new QuantitativeProduct(Guid.Empty, productId, 13) };
        Warehouse warehouse = new(warehouseId, "Source Warehouse", products);

        var getWarehouseServiceStub = new Mock<IGetWarehouseByIdService>();
        getWarehouseServiceStub
            .Setup(x => x.GetAsync(warehouseId))
            .Returns(Task.FromResult(warehouse)!);

        var getProductsTransferServiceStub = new Mock<IGetProductsTransferService>();

        var receiveTransferProducts = new[] { new QuantitativeProductDto(productId, 10) };
        getProductsTransferServiceStub
            .Setup(x => x.GetAsync(warehouseId, time, ProductsTransfer.Receive))
            .Returns(Task.FromResult<IList<ProductsTransfer>>(new[] { new ProductsTransfer(time + 15, Guid.NewGuid(), warehouseId, receiveTransferProducts) }));

        var dispatchTransferProducts = new[] { new QuantitativeProductDto(productId, 7) };
        getProductsTransferServiceStub
            .Setup(x => x.GetAsync(warehouseId, time, ProductsTransfer.Dispatch))
            .Returns(Task.FromResult<IList<ProductsTransfer>>(new[] { new ProductsTransfer(time + 45, warehouseId, Guid.NewGuid(), dispatchTransferProducts) }));

        var sut = new GetWarehouseReportQueryHandler(getWarehouseServiceStub.Object, getProductsTransferServiceStub.Object);

        // Act
        WarehouseReport? result = await sut.HandleAsync(new GetWarehouseReportQuery(warehouseId, time));

        // Assert
        var expected = new WarehouseReport(time, warehouse.Name, new[] { new QuantitativeProductDto(productId, 10) });
        Assert.True(expected.Equals(result));
    }

    [Fact]
    public async void Report_for_non_existent_warehouse_is_fails()
    {
        // Arrange
        Guid warehouseId = Guid.NewGuid();
        Guid productId = Guid.NewGuid();
        long time = 2022_10_28_00_00_00;

        var products = new List<QuantitativeProduct> { new QuantitativeProduct(Guid.Empty, productId, 10) };
        Warehouse warehouse = new(warehouseId, "Source Warehouse", products);

        var getWarehouseServiceStub = new Mock<IGetWarehouseByIdService>();
        getWarehouseServiceStub
            .Setup(x => x.GetAsync(warehouseId))
            .Returns(Task.FromResult<Warehouse?>(null));

        var getProductsTransferServiceStub = new Mock<IGetProductsTransferService>();

        getProductsTransferServiceStub
            .Setup(x => x.GetAsync(warehouseId, time, ProductsTransfer.Receive))
            .Returns(Task.FromResult<IList<ProductsTransfer>>(Enumerable.Empty<ProductsTransfer>().ToList()));

        getProductsTransferServiceStub
            .Setup(x => x.GetAsync(warehouseId, time, ProductsTransfer.Dispatch))
            .Returns(Task.FromResult<IList<ProductsTransfer>>(Enumerable.Empty<ProductsTransfer>().ToList()));

        var sut = new GetWarehouseReportQueryHandler(getWarehouseServiceStub.Object, getProductsTransferServiceStub.Object);

        // Act
        WarehouseReport? result = await sut.HandleAsync(new GetWarehouseReportQuery(warehouseId, time));

        // Assert
        Assert.Null(result);
    }
}
