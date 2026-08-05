using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Seed;

internal static class RoleSeeder
{
    private const string AdminRoleName = "Admin";


    public static async Task SeedAsync(
        ApplicationDbContext context,
        RoleManager<ApplicationRole> roleManager)
    {
        var adminRole =
            await roleManager.FindByNameAsync(AdminRoleName);


        if (adminRole is null)
        {
            adminRole = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = AdminRoleName,
                NormalizedName =
                    AdminRoleName.ToUpperInvariant()
            };


            await roleManager.CreateAsync(adminRole);
        }


        var permissionIds = await context.Permissions
            .Select(x => x.Id)
            .ToListAsync();


        var existingPermissions =
            await context.RolePermissions
                .Where(x => x.RoleId == adminRole.Id)
                .Select(x => x.PermissionId)
                .ToListAsync();


        var newPermissions = permissionIds
            .Except(existingPermissions);


        await context.SaveChangesAsync();
    }
}