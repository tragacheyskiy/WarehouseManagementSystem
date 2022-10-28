using WarehouseManagementSystem.Domain.Queries.Abstractions;

namespace WarehouseManagementSystem.Domain.QueryHandlers.Abstractions;

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
{
    Task<TResult> HandleAsync(TQuery query);
}
