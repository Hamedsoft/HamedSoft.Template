using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Users.GetUserProfile;

public sealed class GetUserProfileQueryHandler
    : IQueryHandler<GetUserProfileQuery, Result<UserProfileDto>>
{
    private readonly IUserManagementService _userManagementService;

    public GetUserProfileQueryHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }


    public async Task<Result<UserProfileDto>> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.GetProfileAsync(
            request.UserId,
            cancellationToken);
    }
}