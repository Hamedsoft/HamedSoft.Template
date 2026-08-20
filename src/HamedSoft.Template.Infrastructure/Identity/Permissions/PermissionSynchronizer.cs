using HamedSoft.Template.Application.Contracts.Permissions;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Permissions;

public sealed class PermissionSynchronizer
{
    private readonly ApplicationDbContext _context;
    private readonly IPermissionDiscoveryService _discoveryService;

    public PermissionSynchronizer(ApplicationDbContext context, IPermissionDiscoveryService discoveryService)
    {
        _context = context;
        _discoveryService = discoveryService;
    }

    public async Task SyncAsync(CancellationToken cancellationToken = default)
    {
        var definitions = _discoveryService.Discover();

        var existingPermissions = await _context.Permissions.ToListAsync(cancellationToken);

        foreach (var definition in definitions)
        {
            var existing = existingPermissions.FirstOrDefault(x => x.Name == definition.Name);

            if (existing is null)
            {
                _context.Permissions.Add(
                    new Permission(
                        Guid.NewGuid(),
                        definition.Name,
                        definition.Module,
                        definition.Category,
                        definition.DisplayName,
                        definition.Description));

                continue;
            }

            existing.UpdateMetadata(
                definition.Module,
                definition.Category,
                definition.DisplayName,
                definition.Description);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}