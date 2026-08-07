using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Users.GetUserSecurity;

public sealed class GetUserSecurityQueryHandler
    : IQueryHandler<GetUserSecurityQuery, Result<UserSecurityDto>>
{
    private readonly IUserManagementService _userManagementService;


    public GetUserSecurityQueryHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }


    public async Task<Result<UserSecurityDto>> Handle(
        GetUserSecurityQuery request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.GetSecurityAsync(
            request.UserId,
            cancellationToken);
    }
}