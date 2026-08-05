using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Contracts.Permissions;

public interface IPermissionService
{
    Task<Result<IReadOnlyList<PermissionDto>>> GetAllAsync(
        CancellationToken cancellationToken = default);
}