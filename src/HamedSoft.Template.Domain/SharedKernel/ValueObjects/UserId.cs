using HamedSoft.Template.Domain.SeedWork;
using System.Diagnostics.CodeAnalysis;

namespace HamedSoft.Template.Domain.SharedKernel.ValueObjects;

public sealed class UserId : StronglyTypedId<Guid, UserId>
{
    /// <summary>
    /// Required by EF Core.
    /// </summary>
    private UserId()
        : base(Guid.Empty)
    {
    }

    private UserId(Guid value)
        : base(value)
    {
    }

    /// <summary>
    /// Creates a new unique identifier.
    /// </summary>
    public static UserId New()
    {
        return new(Guid.NewGuid());
    }

    /// <summary>
    /// Creates an identifier from an existing Guid.
    /// </summary>
    public static UserId Create(Guid value)
    {
        Guard.AgainstDefault(value, nameof(value));

        return new(value);
    }
}
