using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.AssignRoles;

public sealed class AssignRolesHandler
    : ICommandHandler<AssignRolesCommand, Result>
{
    private readonly IUserManagementService _userManagementService;

    public AssignRolesHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result> Handle(
        AssignRolesCommand request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.AssignRolesAsync(
    request.UserId,
    request.RoleIds,
    cancellationToken);
    }
}