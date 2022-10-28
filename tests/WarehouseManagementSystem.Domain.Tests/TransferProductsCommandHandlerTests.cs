using Moq;
using WarehouseManagementSystem.Domain.CommandHandlers;
using WarehouseManagementSystem.Domain.Commands;
using WarehouseManagementSystem.Domain.Dtos;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Services.Abstractions;

namespace WarehouseManagementSystem.Domain.Tests;

public sealed class TransferProductsCommandHandlerTests
{
    [Fact]
    public async void Successful_transfer()
    {
        // Arrange
        Guid sourceWarehouseId = Guid.NewGuid();
        Guid targetWarehouseId = Guid.NewGuid();
        Guid sourceProductId = Guid.NewGuid();

        var sourceProducts = new List<QuantitativeProduct> { new QuantitativeProduct(sourceProductId, 10) };
        Warehouse sourceWarehouse = CreateWarehouse(sourceWarehouseId, sourceProducts);
        Warehouse targetWarehouse = CreateWarehouse(targetWarehouseId, Enumerable.Empty<QuantitativeProduct>().ToList());

        var dateTimeProvider = SetupAndGetDateTimeProvider(2022_10_28_14_30_00);

        var getWarehouseServiceStub = new Mock<IGetWarehouseByIdService>();

        getWarehouseServiceStub
            .Setup(x => x.GetAsync(sourceWarehouseId))
            .Returns(Task.FromResult(sourceWarehouse)!);
        getWarehouseServiceStub
            .Setup(x => x.GetAsync(targetWarehouseId))
            .Returns(Task.FromResult(targetWarehouse)!);

        var updateWarehouseServiceMock = new Mock<IUpdateWarehouseService>();

        var transferProducts = new List<QuantitativeProductDto> { new QuantitativeProductDto(sourceProductId, 7) };
        var command = new TransferProductsCommand(sourceWarehouseId, targetWarehouseId, transferProducts);

        var sut = new TransferProductsCommandHandler(
            getWarehouseServiceStub.Object,
            updateWarehouseServiceMock.Object,
            dateTimeProvider);

        // Act
        ProductsTransfer? result = await sut.HandleAsync(command);

        // Assert
        var expected = new ProductsTransfer(dateTimeProvider.NowUtc, sourceWarehouseId, targetWarehouseId, transferProducts);
        Assert.True(expected.Equals(result));

        updateWarehouseServiceMock.Verify(x => x.UpdateAsync(sourceWarehouse), Times.Once);
        updateWarehouseServiceMock.Verify(x => x.UpdateAsync(targetWarehouse), Times.Once);
    }

    [Fact]
    public async void Transfer_products_with_incorrect_quantity_is_fails()
    {
        // Arrange
        Guid sourceWarehouseId = Guid.NewGuid();
        Guid targetWarehouseId = Guid.NewGuid();
        Guid sourceProductId = Guid.NewGuid();

        var sourceProducts = new List<QuantitativeProduct> { new QuantitativeProduct(sourceProductId, 10) };
        Warehouse sourceWarehouse = CreateWarehouse(sourceWarehouseId, sourceProducts);
        Warehouse targetWarehouse = CreateWarehouse(targetWarehouseId, Enumerable.Empty<QuantitativeProduct>().ToList());

        var dateTimeProvider = SetupAndGetDateTimeProvider(2022_10_28_14_30_00);

        var getWarehouseServiceStub = new Mock<IGetWarehouseByIdService>();

        getWarehouseServiceStub
            .Setup(x => x.GetAsync(sourceWarehouseId))
            .Returns(Task.FromResult(sourceWarehouse)!);
        getWarehouseServiceStub
            .Setup(x => x.GetAsync(targetWarehouseId))
            .Returns(Task.FromResult(targetWarehouse)!);

        var updateWarehouseServiceMock = new Mock<IUpdateWarehouseService>();

        var transferProducts = new List<QuantitativeProductDto> { new QuantitativeProductDto(sourceProductId, 17) };
        var command = new TransferProductsCommand(sourceWarehouseId, targetWarehouseId, transferProducts);

        var sut = new TransferProductsCommandHandler(
            getWarehouseServiceStub.Object,
            updateWarehouseServiceMock.Object,
            dateTimeProvider);

        // Act
        ProductsTransfer? result = await sut.HandleAsync(command);

        // Assert
        Assert.Null(result);

        updateWarehouseServiceMock.Verify(x => x.UpdateAsync(sourceWarehouse), Times.Never);
        updateWarehouseServiceMock.Verify(x => x.UpdateAsync(targetWarehouse), Times.Never);
    }

    [Fact]
    public async void Transfer_non_existent_products_is_fails()
    {
        // Arrange
        Guid sourceWarehouseId = Guid.NewGuid();
        Guid targetWarehouseId = Guid.NewGuid();
        Guid sourceProductId = Guid.NewGuid();

        var sourceProducts = new List<QuantitativeProduct> { new QuantitativeProduct(sourceProductId, 10) };
        Warehouse sourceWarehouse = CreateWarehouse(sourceWarehouseId, sourceProducts);
        Warehouse targetWarehouse = CreateWarehouse(targetWarehouseId, Enumerable.Empty<QuantitativeProduct>().ToList());

        var dateTimeProvider = SetupAndGetDateTimeProvider(2022_10_28_14_30_00);

        var getWarehouseServiceStub = new Mock<IGetWarehouseByIdService>();

        getWarehouseServiceStub
            .Setup(x => x.GetAsync(sourceWarehouseId))
            .Returns(Task.FromResult(sourceWarehouse)!);
        getWarehouseServiceStub
            .Setup(x => x.GetAsync(targetWarehouseId))
            .Returns(Task.FromResult(targetWarehouse)!);

        var updateWarehouseServiceMock = new Mock<IUpdateWarehouseService>();

        var transferProducts = new List<QuantitativeProductDto> { new QuantitativeProductDto(Guid.NewGuid(), 7) };
        var command = new TransferProductsCommand(sourceWarehouseId, targetWarehouseId, transferProducts);

        var sut = new TransferProductsCommandHandler(
            getWarehouseServiceStub.Object,
            updateWarehouseServiceMock.Object,
            dateTimeProvider);

        // Act
        ProductsTransfer? result = await sut.HandleAsync(command);

        // Assert
        Assert.Null(result);

        updateWarehouseServiceMock.Verify(x => x.UpdateAsync(sourceWarehouse), Times.Never);
        updateWarehouseServiceMock.Verify(x => x.UpdateAsync(targetWarehouse), Times.Never);
    }

    [Fact]
    public async void Transfer_duplicate_products_is_fails()
    {
        // Arrange
        Guid sourceWarehouseId = Guid.NewGuid();
        Guid targetWarehouseId = Guid.NewGuid();
        Guid sourceProductId = Guid.NewGuid();

        var sourceProducts = new List<QuantitativeProduct> { new QuantitativeProduct(sourceProductId, 10) };
        Warehouse sourceWarehouse = CreateWarehouse(sourceWarehouseId, sourceProducts);
        Warehouse targetWarehouse = CreateWarehouse(targetWarehouseId, Enumerable.Empty<QuantitativeProduct>().ToList());

        var dateTimeProvider = SetupAndGetDateTimeProvider(2022_10_28_14_30_00);

        var getWarehouseServiceStub = new Mock<IGetWarehouseByIdService>();

        getWarehouseServiceStub
            .Setup(x => x.GetAsync(sourceWarehouseId))
            .Returns(Task.FromResult(sourceWarehouse)!);
        getWarehouseServiceStub
            .Setup(x => x.GetAsync(targetWarehouseId))
            .Returns(Task.FromResult(targetWarehouse)!);

        var updateWarehouseServiceMock = new Mock<IUpdateWarehouseService>();

        var transferProducts = new List<QuantitativeProductDto>
        {
            new QuantitativeProductDto(sourceProductId, 7), new QuantitativeProductDto(sourceProductId, 7)
        };
        var command = new TransferProductsCommand(sourceWarehouseId, targetWarehouseId, transferProducts);

        var sut = new TransferProductsCommandHandler(
            getWarehouseServiceStub.Object,
            updateWarehouseServiceMock.Object,
            dateTimeProvider);

        // Act
        ProductsTransfer? result = await sut.HandleAsync(command);

        // Assert
        Assert.Null(result);

        updateWarehouseServiceMock.Verify(x => x.UpdateAsync(sourceWarehouse), Times.Never);
        updateWarehouseServiceMock.Verify(x => x.UpdateAsync(targetWarehouse), Times.Never);
    }

    private Warehouse CreateWarehouse(Guid id, List<QuantitativeProduct> products)
    {
        return new Warehouse(id, string.Empty, products);
    }

    private IDateTimeProvider SetupAndGetDateTimeProvider(long stubValue)
    {
        var result = new Mock<IDateTimeProvider>();

        result.SetupGet(x => x.Now).Returns(stubValue);
        result.SetupGet(x => x.NowUtc).Returns(stubValue);

        return result.Object;
    }
}