using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Contracts.Repositories.Reads;

public interface IRoleReadRepository
{
    Task<bool> ExistsAsync(Guid roleId, CancellationToken cancellationToken = default);

    Task<bool> IsAdminAsync(Guid roleId, CancellationToken cancellationToken = default);

    Task<PagedResult<RoleDto>> GetPagedAsync(bool includeAdmin, int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);

    Task<RolePermissionsDto?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default);
}