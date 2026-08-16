using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Web.ViewModels.Common.Pagination;

namespace HamedSoft.Template.Web.ViewModels.Users;

public sealed class UsersIndexViewModel
{
    public PaginationViewModel ActivePagination { get; init; } = default!;

    public PaginationViewModel InactivePagination { get; init; } = default!;

    public PaginationViewModel LockedPagination { get; init; } = default!;

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

    public string ActiveTab { get; init; } = "active";

}