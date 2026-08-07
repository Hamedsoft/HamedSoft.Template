using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.UpdateProfile;

public sealed record UpdateUserProfileCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string? Email,
    string? PhoneNumber)
    : ICommand<Result>;