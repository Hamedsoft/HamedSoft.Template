using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HamedSoft.Template.Infrastructure.Identity.Seed;

internal static class AdminUserSeeder
{
    private const string AdminRole = "Admin";


    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        IOptions<AdminUserOptions> options)
    {
        var adminOptions = options.Value;


        var user = await userManager
            .FindByNameAsync(adminOptions.UserName);


        if (user is null)
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = adminOptions.UserName,
                Email = adminOptions.UserName,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager
                .CreateAsync(
                    user,
                    adminOptions.Password);


            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(
                        Environment.NewLine,
                        result.Errors.Select(x => x.Description)));
            }
        }


        if (!await userManager.IsInRoleAsync(
                user,
                AdminRole))
        {
            await userManager.AddToRoleAsync(
                user,
                AdminRole);
        }
    }
}