using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.LockUser;

public sealed record LockUserCommand(
    Guid UserId)
    : ICommand<Result>;