using WarehouseManagementSystem.Domain.Queries.Abstractions;

namespace WarehouseManagementSystem.Domain.Queries;

public sealed class GetWarehouseReportQuery : IQuery
{
	public GetWarehouseReportQuery(Guid warehouseId, long time)
	{
		WarehouseId = warehouseId;
		Time = time;
	}

	public Guid WarehouseId { get; }
	public long Time { get; }
}
