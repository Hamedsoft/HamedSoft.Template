using HamedSoft.Template.Infrastructure.Identity.Permissions;

namespace HamedSoft.Template.Infrastructure.Initialization;

public sealed class PermissionInitializer
    : IInitializer
{
    private readonly PermissionSynchronizer _permissionSynchronizer;

    public PermissionInitializer(
        PermissionSynchronizer permissionSynchronizer)
    {
        _permissionSynchronizer = permissionSynchronizer;
    }

    public async Task InitializeAsync(
        CancellationToken cancellationToken = default)
    {
        await _permissionSynchronizer.SyncAsync(
            cancellationToken);
    }
}