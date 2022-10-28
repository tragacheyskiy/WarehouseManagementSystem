using WarehouseManagementSystem.Domain.CommandHandlers.Abstractions;
using WarehouseManagementSystem.Domain.Commands;
using WarehouseManagementSystem.Domain.Dtos;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Services.Abstractions;

namespace WarehouseManagementSystem.Domain.CommandHandlers;

public sealed class TransferProductsCommandHandler : ICommandHandler<TransferProductsCommand, ProductsTransfer?>
{
    private readonly IGetWarehouseByIdService _getWarehouseService;
    private readonly IUpdateWarehouseService _updateWarehouseService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public TransferProductsCommandHandler(
        IGetWarehouseByIdService getWarehouseService,
        IUpdateWarehouseService updateWarehouseService,
        IDateTimeProvider dateTimeProvider)
    {
        _getWarehouseService = getWarehouseService ?? throw new ArgumentNullException(nameof(getWarehouseService));
        _updateWarehouseService = updateWarehouseService ?? throw new ArgumentNullException(nameof(updateWarehouseService));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public async Task<ProductsTransfer?> HandleAsync(TransferProductsCommand command)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        if (HasDublicates(command.Products))
            return null;

        Warehouse sourceWarehouse = (await _getWarehouseService.GetAsync(command.SourceWarehouseId))!;
        Warehouse targetWarehouse = (await _getWarehouseService.GetAsync(command.TargetWarehouseId))!;

        if (!sourceWarehouse.DispatchProducts(command.Products) || !targetWarehouse.ReceiveProducts(command.Products))
        {
            return null;
        }

        await Task.WhenAll(
            _updateWarehouseService.UpdateAsync(sourceWarehouse),
            _updateWarehouseService.UpdateAsync(targetWarehouse));

        return new ProductsTransfer(_dateTimeProvider.NowUtc, command.SourceWarehouseId, command.TargetWarehouseId, command.Products);
    }

    private bool HasDublicates(IReadOnlyList<QuantitativeProductDto> transferProducts)
    {
        return transferProducts.DistinctBy(x => x.ProductId).Count() != transferProducts.Count;
    }
}
