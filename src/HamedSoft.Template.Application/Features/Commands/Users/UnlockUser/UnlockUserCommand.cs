using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.UnlockUser;

public sealed record UnlockUserCommand(
    Guid UserId)
    : ICommand<Result>;