namespace HamedSoft.Template.Domain.SeedWork;

/// <summary>
/// Represents a value object that wraps a single value.
/// </summary>
public abstract class SingleValueObject<TValue, TSelf>
    : ValueObject<TSelf>
    where TValue : notnull
    where TSelf : SingleValueObject<TValue, TSelf>
{
    protected SingleValueObject(TValue value)
    {
        Value = value;
    }

    public TValue Value { get; }

    protected sealed override bool EqualsCore(TSelf other)
    {
        return EqualityComparer<TValue>.Default.Equals(
            Value,
            other.Value);
    }

    protected sealed override void AddHashCode(ref HashCode hash)
    {
        hash.Add(Value);
    }

    public override string ToString()
    {
        return Value.ToString() ?? string.Empty;
    }

    public static implicit operator TValue(
        SingleValueObject<TValue, TSelf> value)
    {
        return value.Value;
    }
}