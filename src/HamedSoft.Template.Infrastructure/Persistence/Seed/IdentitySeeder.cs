using HamedSoft.Template.Infrastructure.Persistence;
using HamedSoft.Template.Infrastructure.Identity.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HamedSoft.Template.Infrastructure.Identity.Models;

namespace HamedSoft.Template.Infrastructure.Persistence.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
    {
        await context.Database.MigrateAsync();

        var permissions = new[]
        {
            PermissionConstants.UsersView,
            PermissionConstants.UsersCreate,
            PermissionConstants.UsersEdit,
            PermissionConstants.UsersDelete,
            PermissionConstants.RolesManage
        };

        foreach (var permissionName in permissions)
        {
            if (!await context.Permissions.AnyAsync(x => x.Name == permissionName))
                context.Permissions.Add(new Permission(permissionName));
        }

        await context.SaveChangesAsync();

        var adminRole = await roleManager.FindByNameAsync("Admin");

        if (adminRole == null)
        {
            adminRole = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            await roleManager.CreateAsync(adminRole);
        }


        var allPermissions = await context.Permissions.ToListAsync();

        foreach (var permission in allPermissions)
        {
            var exists = await context.RolePermissions.AnyAsync(x => x.RoleId == adminRole.Id && x.PermissionId == permission.Id);

            if (!exists)
            {
                context.RolePermissions.Add(new RolePermission(adminRole.Id, permission.Id));
            }
        }

        await context.SaveChangesAsync();
    }
}