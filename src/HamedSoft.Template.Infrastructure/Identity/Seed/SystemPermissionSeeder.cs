using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Seed;

public static class SystemPermissionSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, string roleName)
    {
        var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);

        if (role is null)
            return;

        var permissions = await context.Permissions.ToListAsync();

        var existingPermissions = await context.RolePermissions.Where(x => x.RoleId == role.Id).Select(x => x.PermissionId).ToListAsync();

        var missingPermissions = permissions.Where(x => !existingPermissions.Contains(x.Id)).ToList();

        if (missingPermissions.Count == 0)
            return;

        foreach (var permission in missingPermissions)
        {
            context.RolePermissions.Add(new RolePermission(role.Id, permission.Id));
        }

        await context.SaveChangesAsync();
    }
}