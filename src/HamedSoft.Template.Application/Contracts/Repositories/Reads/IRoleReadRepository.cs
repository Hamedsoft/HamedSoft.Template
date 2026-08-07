using HamedSoft.Template.Application.Contracts.Roles;

namespace HamedSoft.Template.Application.Contracts.Repositories.Reads;

public interface IRoleReadRepository
{
    Task<bool> ExistsAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);


    Task<IReadOnlyList<RolePermissionsDto>> GetAllAsync(
        CancellationToken cancellationToken = default);


    Task<RolePermissionsDto?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);
}