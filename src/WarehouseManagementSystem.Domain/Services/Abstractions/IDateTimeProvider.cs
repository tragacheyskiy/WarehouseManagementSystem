namespace WarehouseManagementSystem.Domain.Services.Abstractions;

public interface IDateTimeProvider
{
    long Now { get; }
    long NowUtc { get; }
}
