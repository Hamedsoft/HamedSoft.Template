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

        var permissions = new HashSet<string>
        {
            PermissionConstants.Users.View,
            PermissionConstants.Users.Create,
            PermissionConstants.Users.Edit,
            PermissionConstants.Users.Delete,

            PermissionConstants.Roles.View,
            PermissionConstants.Roles.Create,
            PermissionConstants.Roles.Delete,

            PermissionConstants.Settings.View,
            PermissionConstants.Settings.Edit
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

        var adminUser = await context.Users
    .FirstOrDefaultAsync(x => x.UserName == "admin");

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                NormalizedUserName = "ADMIN"
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            adminUser.PasswordHash = passwordHasher.HashPassword(
                adminUser,
                "Admin@123");

            context.Users.Add(adminUser);

            await context.SaveChangesAsync();
        }

        var userHasRole = await context.UserRoles
            .AnyAsync(x =>
                x.UserId == adminUser.Id &&
                x.RoleId == adminRole.Id);

        if (!userHasRole)
        {
            context.UserRoles.Add(new IdentityUserRole<Guid>
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });
        }

        await context.SaveChangesAsync();

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