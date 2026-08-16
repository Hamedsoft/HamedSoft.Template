using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Users;

namespace HamedSoft.Template.Web.ViewModels.Users;

public sealed class UsersIndexViewModel
{
    public PagedResult<UserListItem> ActiveUsers { get; init; } =
        new(
            Array.Empty<UserListItem>(),
            1,
            10,
            0);

    public PagedResult<UserListItem> InactiveUsers { get; init; } =
        new(
            Array.Empty<UserListItem>(),
            1,
            10,
            0);

    public PagedResult<UserListItem> LockedUsers { get; init; } =
        new(
            Array.Empty<UserListItem>(),
            1,
            10,
            0);

    public string? Search { get; init; }

    public int PageSize { get; init; } = 10;
}