namespace HamedSoft.Template.Application.Abstractions.Common;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}