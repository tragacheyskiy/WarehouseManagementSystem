using WarehouseManagementSystem.Domain.Dtos;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.Domain.QueryHandlers.Abstractions;
using WarehouseManagementSystem.Domain.Services.Abstractions;

namespace WarehouseManagementSystem.Domain.QueryHandlers;

public sealed class GetWarehouseReportQueryHandler : IQueryHandler<GetWarehouseReportQuery, WarehouseReport?>
{
    private readonly IGetWarehouseByIdService _getWarehouseService;
    private readonly IGetProductsTransferService _getProductsTransferService;

    public GetWarehouseReportQueryHandler(
        IGetWarehouseByIdService getWarehouseService,
        IGetProductsTransferService getProductsTransferService)
    {
        _getWarehouseService = getWarehouseService ?? throw new ArgumentNullException(nameof(getWarehouseService));
        _getProductsTransferService = getProductsTransferService ?? throw new ArgumentNullException(nameof(getProductsTransferService));
    }

    public async Task<WarehouseReport?> HandleAsync(GetWarehouseReportQuery query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        Warehouse? warehouse = await _getWarehouseService.GetAsync(query.WarehouseId);

        if (warehouse is null)
            return null;

        IList<ProductsTransfer> receiveTransfers = await _getProductsTransferService
            .GetAsync(query.WarehouseId, query.Time, ProductsTransfer.Receive);

        IList<ProductsTransfer> dispatchTransfers = await _getProductsTransferService
            .GetAsync(query.WarehouseId, query.Time, ProductsTransfer.Dispatch);

        foreach (ProductsTransfer transfer in receiveTransfers)
        {
            if (!warehouse.ReceiveProducts(transfer.Products))
                return null;
        }

        foreach (ProductsTransfer transfer in dispatchTransfers)
        {
            if (!warehouse.DispatchProducts(transfer.Products))
                return null;
        }

        List<QuantitativeProductDto> products = warehouse.Products.Select(x => new QuantitativeProductDto(x.ProductId, x.Quantity)).ToList();

        return new WarehouseReport(query.Time, warehouse.Name, products);
    }
}
