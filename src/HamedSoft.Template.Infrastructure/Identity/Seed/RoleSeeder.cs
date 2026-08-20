using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Seed;

internal static class RoleSeeder
{
    private const string AdminRoleName = SystemRoles.Admin;
    private const string ManagerRoleName = SystemRoles.Manager;

    public static async Task SeedAsync(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
    {
        await SeedRole(roleManager, AdminRoleName);
        await SeedRole(roleManager, ManagerRoleName);
        await context.SaveChangesAsync();
    }
    private static async Task SeedRole(RoleManager<ApplicationRole> roleManager, string RoleName)
    {
        var role = await roleManager.FindByNameAsync(RoleName);
        if (role is null)
        {
            role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = RoleName,
                NormalizedName = RoleName.ToUpperInvariant()
            };

            await roleManager.CreateAsync(role);
        }
    }
}