using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Users.GetUsers;

public sealed class GetUsersHandler : IQueryHandler<GetUsersQuery, Result<IReadOnlyList<UserListItem>>>
{
    private readonly IUserManagementService _userManagementService;

    public GetUsersHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<IReadOnlyList<UserListItem>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userManagementService.GetAllAsync(cancellationToken);
    }
}