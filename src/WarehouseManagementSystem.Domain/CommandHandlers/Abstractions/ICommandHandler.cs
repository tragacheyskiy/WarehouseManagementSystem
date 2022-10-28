using WarehouseManagementSystem.Domain.Commands.Abstractions;

namespace WarehouseManagementSystem.Domain.CommandHandlers.Abstractions;

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
{
    Task<TResult> HandleAsync(TCommand command);
}
