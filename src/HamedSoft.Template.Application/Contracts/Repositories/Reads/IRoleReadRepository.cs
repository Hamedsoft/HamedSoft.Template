using HamedSoft.Template.Application.Contracts.Roles;

namespace HamedSoft.Template.Application.Contracts.Repositories.Reads;

public interface IRoleReadRepository
{
    Task<bool> ExistsAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);

    Task<bool> IsAdminAsync(
    Guid roleId,
    CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RoleDto>> GetAllAsync(bool withAdmin,
        CancellationToken cancellationToken = default);

    Task<RolePermissionsDto?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);
}