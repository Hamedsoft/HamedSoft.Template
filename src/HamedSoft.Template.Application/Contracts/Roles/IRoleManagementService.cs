using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Contracts.Roles;

public interface IRoleManagementService
{
    Task<Result<IReadOnlyList<RolePermissionsDto>>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Result<RolePermissionsDto>> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);

    Task<Result> AssignPermissionsAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default);
}