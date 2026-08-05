using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Roles.AssignPermissions;

public sealed record AssignPermissionsCommand(
    Guid RoleId,
    IReadOnlyCollection<Guid> PermissionIds)
    : ICommand<Result>;