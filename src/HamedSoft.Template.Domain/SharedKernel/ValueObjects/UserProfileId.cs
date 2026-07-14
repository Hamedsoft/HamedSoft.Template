using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Domain.SharedKernel.ValueObjects;

/// <summary>
/// Represents the unique identifier of a UserProfile.
/// </summary>
public sealed class UserProfileId : StronglyTypedId<Guid, UserProfileId>
{
    /// <summary>
    /// Required by EF Core.
    /// </summary>
    private UserProfileId()
        : base(Guid.Empty)
    {
    }

    private UserProfileId(Guid value)
        : base(value)
    {
    }

    /// <summary>
    /// Creates a new unique identifier.
    /// </summary>
    public static UserProfileId New()
    {
        return new(Guid.NewGuid());
    }

    /// <summary>
    /// Creates an identifier from an existing Guid.
    /// </summary>
    public static UserProfileId Create(Guid value)
    {
        Guard.AgainstDefault(value, nameof(value));

        return new(value);
    }
}