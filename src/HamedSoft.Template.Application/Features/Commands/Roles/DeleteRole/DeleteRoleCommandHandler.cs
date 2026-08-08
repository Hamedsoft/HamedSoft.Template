using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Domain.SeedWork;
using MediatR;

namespace HamedSoft.Template.Application.Features.Commands.Roles.DeleteRole;

public sealed class DeleteRoleCommandHandler
    : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly IRoleManagementService _roleManagementService;

    public DeleteRoleCommandHandler(
        IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }

    public async Task<Result> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        return await _roleManagementService.DeleteAsync(
            request.RoleId,
            cancellationToken);
    }
}