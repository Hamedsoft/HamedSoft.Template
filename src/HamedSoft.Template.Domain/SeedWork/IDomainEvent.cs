namespace HamedSoft.Template.Domain.SeedWork;

/// <summary>
/// Represents a domain event.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}