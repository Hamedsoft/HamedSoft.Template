using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Roles.AssignPermissions;

public sealed class AssignPermissionsHandler
    : ICommandHandler<AssignPermissionsCommand, Result>
{
    private readonly IRoleManagementService _roleManagementService;


    public AssignPermissionsHandler(
        IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }


    public async Task<Result> Handle(
        AssignPermissionsCommand request,
        CancellationToken cancellationToken)
    {
        return await _roleManagementService
            .AssignPermissionsAsync(
                request.RoleId,
                request.PermissionIds,
                cancellationToken);
    }
}