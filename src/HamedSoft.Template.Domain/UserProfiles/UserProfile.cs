using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Domain.UserProfiles.Events;

namespace HamedSoft.Template.Domain.UserProfiles;

/// <summary>
/// Represents the UserProfile aggregate root.
/// </summary>
public sealed class UserProfile : AggregateRoot<UserProfileId>
{
    /// <summary>
    /// Required by EF Core.
    /// </summary>
    private UserProfile()
    {
    }

    private UserProfile(UserProfileId id)
        : base(id)
    {
    }

    public string FirstName { get;private set; } = string.Empty;
    public string LastName { get;private set; } = string.Empty;

    /// <summary>
    /// Creates a new UserProfile aggregate.
    /// </summary>
    public static UserProfile Create(UserProfileId id,string firstName,string lastName)
    {
        Guard.AgainstNull(id, nameof(id));
        Guard.AgainstNull(firstName, nameof(firstName));
        Guard.AgainstNull(lastName, nameof(lastName));

        UserProfile userProfile = new(id) { FirstName=firstName,LastName=lastName};

        userProfile.Raise(
            new UserProfileCreatedDomainEvent(id));

        return userProfile;
    }
}