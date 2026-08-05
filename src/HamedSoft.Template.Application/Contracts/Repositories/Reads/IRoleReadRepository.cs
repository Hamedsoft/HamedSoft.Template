using HamedSoft.Template.Application.Contracts.Roles;

namespace HamedSoft.Template.Application.Contracts.Repositories.Reads;

public interface IRoleReadRepository
{
    Task<bool> ExistsAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);


    Task<IReadOnlyList<RoleDto>> GetAllAsync(
        CancellationToken cancellationToken = default);


    Task<RoleDto?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);
}