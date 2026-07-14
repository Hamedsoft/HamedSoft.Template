namespace HamedSoft.Template.Domain.UserProfiles.Events;

using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;

/// <summary>
/// Raised when a new user profile is created.
/// </summary>
public sealed record UserProfileCreatedDomainEvent(
    UserProfileId UserProfileId)
    : DomainEvent;