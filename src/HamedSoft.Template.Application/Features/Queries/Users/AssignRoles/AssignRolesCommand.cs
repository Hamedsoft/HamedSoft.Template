using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.AssignRoles;

public sealed record AssignRolesCommand(
    Guid UserId,
    IReadOnlyCollection<string> RoleNames)
    : ICommand<Result>;