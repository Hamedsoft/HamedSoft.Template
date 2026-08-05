namespace HamedSoft.Template.Application.Contracts.Repositories.Writes;

public interface IRoleWriteRepository
{
    Task ReplacePermissionsAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default);
}