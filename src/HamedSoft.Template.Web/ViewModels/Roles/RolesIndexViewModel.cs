using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Web.ViewModels.Common.Pagination;

namespace HamedSoft.Template.Web.ViewModels.Roles;

public sealed class RolesIndexViewModel
{
    public PagedResult<RoleDto> Roles { get; init; } = new(Array.Empty<RoleDto>(), pageNumber: 1, pageSize: 10, totalCount: 0);

    public PaginationViewModel Pagination { get; init; } = new();

    public string? Search { get; init; }

    public int PageSize { get; init; } = 10;
}