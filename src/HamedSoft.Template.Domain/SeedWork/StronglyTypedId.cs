namespace HamedSoft.Template.Domain.SeedWork;

public abstract class StronglyTypedId<TValue, TSelf>
    : SingleValueObject<TValue, TSelf>
    where TValue : notnull
    where TSelf : StronglyTypedId<TValue, TSelf>
{
    protected StronglyTypedId(TValue value)
        : base(value)
    {
    }
}