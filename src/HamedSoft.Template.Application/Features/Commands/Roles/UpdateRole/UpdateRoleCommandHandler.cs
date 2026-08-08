using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Domain.SeedWork;
using MediatR;

namespace HamedSoft.Template.Application.Features.Commands.Roles.UpdateRole;

public sealed class UpdateRoleCommandHandler
    : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly IRoleManagementService _roleManagementService;

    public UpdateRoleCommandHandler(
        IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }

    public async Task<Result> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        return await _roleManagementService.UpdateAsync(
            request.RoleId,
            request.RoleName,
            cancellationToken);
    }
}