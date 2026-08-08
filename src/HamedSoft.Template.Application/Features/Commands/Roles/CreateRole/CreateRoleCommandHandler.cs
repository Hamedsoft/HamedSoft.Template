using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Domain.SeedWork;
using MediatR;

namespace HamedSoft.Template.Application.Features.Commands.Roles.CreateRole;

public sealed class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, Result<Guid>>
{
    private readonly IRoleManagementService _roleManagementService;

    public CreateRoleCommandHandler(
        IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }

    public async Task<Result<Guid>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        return await _roleManagementService.CreateAsync(
            request.RoleName,
            cancellationToken);
    }
}