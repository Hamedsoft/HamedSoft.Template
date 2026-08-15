using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Contracts.Roles;

public interface IRoleManagementService
{
    Task<Result<IReadOnlyList<RoleDto>>> GetAllAsync(bool withAdmin,
        CancellationToken cancellationToken = default);

    Task<Result<RolePermissionsDto>> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);

    Task<Result<Guid>> CreateAsync(
        string roleName,
        CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(
        Guid roleId,
        string roleName,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);

    Task<Result> AssignPermissionsAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default);
}