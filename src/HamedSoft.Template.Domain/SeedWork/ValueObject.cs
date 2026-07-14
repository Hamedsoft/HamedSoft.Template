using System.Diagnostics.CodeAnalysis;

namespace HamedSoft.Template.Domain.SeedWork;

/// <summary>
/// Base class for all Value Objects.
/// </summary>
public abstract class ValueObject<TSelf> : IEquatable<TSelf>
    where TSelf : ValueObject<TSelf>
{
    public sealed override bool Equals(object? obj)
    {
        return obj is TSelf other && Equals(other);
    }

    public bool Equals(TSelf? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return EqualsCore(other);
    }

    public sealed override int GetHashCode()
    {
        HashCode hash = new();

        AddHashCode(ref hash);

        return hash.ToHashCode();
    }

    protected abstract bool EqualsCore(
        [DisallowNull] TSelf other);

    protected abstract void AddHashCode(
        ref HashCode hash);

    public static bool operator ==(
        ValueObject<TSelf>? left,
        ValueObject<TSelf>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        ValueObject<TSelf>? left,
        ValueObject<TSelf>? right)
    {
        return !(left == right);
    }
}