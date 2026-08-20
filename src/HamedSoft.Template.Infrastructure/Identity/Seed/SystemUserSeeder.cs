using HamedSoft.Template.Infrastructure.Identity.Options;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HamedSoft.Template.Infrastructure.Identity.Seed;

internal static class SystemUserSeeder
{
    public static async Task<IEnumerable<SystemUserSeedCreationResult>> SeedAsync(UserManager<ApplicationUser> userManager, IOptions<AdminUserOptions> adminoptions, IOptions<ManagerUserOptions> managerOptions)
    {
        var result = new List<SystemUserSeedCreationResult>();
        var adminUser = await userManager.FindByNameAsync(adminoptions.Value.UserName);
        bool UserNotExists = (adminUser == null);

        if (adminUser == null)
            adminUser = await CreateApplicationUser(userManager, adminoptions.Value.UserName, adminoptions.Value.Password);

        if (!await userManager.IsInRoleAsync(adminUser, SystemRoles.Admin))
        {
            await userManager.AddToRoleAsync(adminUser, SystemRoles.Admin);
        }
        result.Add(new SystemUserSeedCreationResult(adminUser, UserNotExists));

        var managerUser = await userManager.FindByNameAsync(managerOptions.Value.UserName);
        UserNotExists = (managerUser == null);

        if (managerUser == null)
            managerUser = await CreateApplicationUser(userManager, managerOptions.Value.UserName, managerOptions.Value.Password);

        if (!await userManager.IsInRoleAsync(managerUser, SystemRoles.Manager))
        {
            await userManager.AddToRoleAsync(managerUser, SystemRoles.Manager);
        }
        result.Add(new SystemUserSeedCreationResult(managerUser, UserNotExists));
        return result;
    }
    private static async Task<ApplicationUser> CreateApplicationUser(UserManager<ApplicationUser> userManager, string username, string password)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Email = username,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        }
        return user;
    }
}