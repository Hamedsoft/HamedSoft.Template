using HamedSoft.Template.Application.Contracts.Permissions;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Permissions;

public sealed class PermissionSynchronizer
{
    private readonly ApplicationDbContext _context;
    private readonly IPermissionDiscoveryService _discoveryService;

    public PermissionSynchronizer(
        ApplicationDbContext context,
        IPermissionDiscoveryService discoveryService)
    {
        _context = context;
        _discoveryService = discoveryService;
    }


    public async Task SyncAsync(
        CancellationToken cancellationToken = default)
    {
        var discoveredPermissions =
            _discoveryService.Discover();


        var existingPermissions =
            await _context.Permissions
                .Select(x => x.Name)
                .ToListAsync(cancellationToken);


        var newPermissions =
            discoveredPermissions
                .Except(existingPermissions)
                .ToList();


        if (newPermissions.Count == 0)
            return;


        foreach (var permissionName in newPermissions)
        {
            _context.Permissions.Add(
                new Permission(permissionName));
        }


        await _context.SaveChangesAsync(
            cancellationToken);
    }
}