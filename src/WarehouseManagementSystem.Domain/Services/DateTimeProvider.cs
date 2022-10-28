using WarehouseManagementSystem.Domain.Services.Abstractions;

namespace WarehouseManagementSystem.Domain.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public const string TimeFormat = "yyyyMMddHHmmss";

    public long Now => long.Parse(DateTime.Now.ToString(TimeFormat));

    public long NowUtc => long.Parse(DateTime.UtcNow.ToString(TimeFormat));
}
