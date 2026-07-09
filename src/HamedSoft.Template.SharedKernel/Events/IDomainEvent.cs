namespace HamedSoft.Template.SharedKernel.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}