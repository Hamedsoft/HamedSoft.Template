using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.UpdateUserStatus;

public sealed record UpdateUserStatusCommand(
    Guid UserId,
    bool IsActive)
    : ICommand<Result>;