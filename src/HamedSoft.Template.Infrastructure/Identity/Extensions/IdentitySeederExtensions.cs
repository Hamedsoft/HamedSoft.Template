using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Identity.Options;
using HamedSoft.Template.Infrastructure.Identity.Permissions;
using HamedSoft.Template.Infrastructure.Identity.Seed;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HamedSoft.Template.Infrastructure.Identity.Extensions;

public static class IdentitySeederExtensions
{
    public static async Task SeedIdentityAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        await RoleSeeder.SeedAsync(context, roleManager);

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var adminOptions = scope.ServiceProvider.GetRequiredService<IOptions<AdminUserOptions>>();
        var managerOptions = scope.ServiceProvider.GetRequiredService<IOptions<ManagerUserOptions>>();
        await SystemUserSeeder.SeedAsync(userManager, adminOptions, managerOptions);

        var permissionSynchronizer = scope.ServiceProvider.GetRequiredService<PermissionSynchronizer>();
        await permissionSynchronizer.SyncAsync();

        await SystemPermissionSeeder.SeedAsync(context, SystemRoles.Admin);
        await SystemPermissionSeeder.SeedAsync(context, SystemRoles.Manager);
    }

}