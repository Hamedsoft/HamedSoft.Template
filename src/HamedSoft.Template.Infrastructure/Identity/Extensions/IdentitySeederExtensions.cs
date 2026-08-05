using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Identity.Options;
using HamedSoft.Template.Infrastructure.Identity.Seed;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HamedSoft.Template.Infrastructure.Identity.Extensions;

public static class IdentitySeederExtensions
{
    public static async Task SeedIdentityAsync(
    this IServiceProvider services)
    {
        using var scope = services.CreateScope();


        var context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<ApplicationRole>>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();


        await PermissionSeeder.SeedAsync(context);


        await RoleSeeder.SeedAsync(
            context,
            roleManager);


        var options = scope.ServiceProvider
            .GetRequiredService<IOptions<AdminUserOptions>>();


        await AdminUserSeeder.SeedAsync(
            userManager,
            options);
    }
}