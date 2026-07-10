using HamedSoft.Template.Application.Abstractions.Common;

namespace HamedSoft.Template.Infrastructure.Common;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow
        => DateTimeOffset.UtcNow;
}