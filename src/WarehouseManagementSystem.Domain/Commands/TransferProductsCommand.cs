using WarehouseManagementSystem.Domain.Commands.Abstractions;
using WarehouseManagementSystem.Domain.Dtos;

namespace WarehouseManagementSystem.Domain.Commands;

public sealed class TransferProductsCommand : ICommand
{
	public TransferProductsCommand(Guid sourceWarehouseId, Guid targetWarehouseId, IReadOnlyList<QuantitativeProductDto> products)
	{
		SourceWarehouseId = sourceWarehouseId;
		TargetWarehouseId = targetWarehouseId;
		Products = products ?? throw new ArgumentNullException(nameof(products));
	}

	public Guid SourceWarehouseId { get; }
	public Guid TargetWarehouseId { get; }
	public IReadOnlyList<QuantitativeProductDto> Products { get; }
}
