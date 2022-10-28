using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Domain.CommandHandlers.Abstractions;
using WarehouseManagementSystem.Domain.Commands;
using WarehouseManagementSystem.Domain.Dtos;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.Domain.QueryHandlers.Abstractions;
using WarehouseManagementSystem.Domain.Services.Abstractions;
using WarehouseManagementSystem.Dtos;

namespace WarehouseManagementSystem.Controllers;

[Route("api/warehouses")]
public sealed class WarehouseController : BaseController
{
    private readonly IGetWarehouseService _getWarehouseService;
    private readonly ICreateProductsTransferService _createProductsTransferService;
    private readonly ICommandHandler<TransferProductsCommand, ProductsTransfer> _transferProductsCommandHandler;
    private readonly IQueryHandler<GetWarehouseReportQuery, WarehouseReport?> _getWarehouseReportQueryHandler;

    public WarehouseController(
        IGetWarehouseService getWarehouseService,
        ICreateProductsTransferService createProductsTransferService,
        ICommandHandler<TransferProductsCommand, ProductsTransfer> transferProductsCommandHandler,
        IQueryHandler<GetWarehouseReportQuery, WarehouseReport?> getWarehouseReportQueryHandler)
    {
        _getWarehouseService = getWarehouseService ?? throw new ArgumentNullException(nameof(getWarehouseService));
        _createProductsTransferService = createProductsTransferService ?? throw new ArgumentNullException(nameof(createProductsTransferService));
        _transferProductsCommandHandler = transferProductsCommandHandler ?? throw new ArgumentNullException(nameof(transferProductsCommandHandler));
        _getWarehouseReportQueryHandler = getWarehouseReportQueryHandler ?? throw new ArgumentNullException(nameof(getWarehouseReportQueryHandler));
    }

    [HttpGet]
    public async Task<DataResult<WarehouseDto>> Get()
    {
        IList<Warehouse> result = await _getWarehouseService.GetAsync();
        IList<WarehouseDto> resultDto = result
            .Select(x => new WarehouseDto
            {
                Id = x.Id,
                Name = x.Name,
                Products = x.Products
                .Select(x => new QuantitativeProductDto(x.ProductId, x.Quantity))
                .ToList()
            })
            .ToList();

        return new DataResult<WarehouseDto>(resultDto);
    }

    [HttpGet("report")]
    public async Task<ActionResult<WarehouseReport>> GetReport([FromQuery] Guid warehouseId, [FromQuery] long time)
    {
        WarehouseReport? result = await _getWarehouseReportQueryHandler.HandleAsync(new GetWarehouseReportQuery(warehouseId, time));

        if (result is null)
            return BadRequest();

        return result!;
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> PostTransfer([FromBody] ProductsTransferRequestDto transferRequest)
    {
        var command = new TransferProductsCommand(transferRequest.SourceWarehouseId, transferRequest.TargetWarehouseId, transferRequest.Products);
        ProductsTransfer? result = await _transferProductsCommandHandler.HandleAsync(command);

        if (result is null)
            return BadRequest();

        await _createProductsTransferService.CreateAsync(result);

        return Ok();
    }
}
