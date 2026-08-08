namespace HamedSoft.Template.Application.Contracts.Repositories.Reads;

public interface IPermissionReadRepository
{
    Task<bool> AllExistAsync(
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default);
}