using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Users.GetUsers;

public sealed class GetUsersHandler
    : IQueryHandler<GetUsersQuery, Result<PagedResult<UserListItem>>>
{
    private readonly IUserManagementService _userManagementService;

    public GetUsersHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<PagedResult<UserListItem>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.GetAllAsync(
            withAdminUser: false,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            search: request.Search,
            status: request.Status,
            cancellationToken: cancellationToken);
    }
}