using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Users.GetUserRoles;

public sealed class GetUserRolesHandler
    : IQueryHandler<GetUserRolesQuery, Result<UserRolesDto>>
{
    private readonly IUserManagementService _userManagementService;

    public GetUserRolesHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<UserRolesDto>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.GetRolesAsync(
            request.UserId,
            cancellationToken);
    }
}