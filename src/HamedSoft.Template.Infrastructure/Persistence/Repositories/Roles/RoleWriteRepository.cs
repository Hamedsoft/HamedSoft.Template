using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;

namespace HamedSoft.Template.Infrastructure.Repositories.Roles;

internal sealed class RoleWriteRepository : IRoleWriteRepository
{
    private readonly ApplicationDbContext _context;

    public RoleWriteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task ReplacePermissionsAsync(
        Guid roleId,
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default)
    {
        var currentPermissions = _context.RolePermissions
            .Where(x => x.RoleId == roleId);

        _context.RolePermissions.RemoveRange(currentPermissions);

        foreach (var permissionId in permissionIds.Distinct())
        {
            _context.RolePermissions.Add(
                new RolePermission(roleId, permissionId));
        }

        return Task.CompletedTask;
    }
}