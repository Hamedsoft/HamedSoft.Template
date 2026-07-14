namespace HamedSoft.Template.Domain.SeedWork;

/// <summary>
/// Represents the base class for domain events.
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        OccurredOnUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the UTC time at which the event occurred.
    /// </summary>
    public DateTime OccurredOnUtc { get; }
}