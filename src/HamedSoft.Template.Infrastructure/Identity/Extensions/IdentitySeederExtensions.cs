using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Identity.Seed;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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


        await PermissionSeeder.SeedAsync(context);

        await RoleSeeder.SeedAsync(
            context,
            roleManager);
    }
}