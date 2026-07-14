using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using HamedSoft.Template.Infrastructure.Identity.Models;

namespace HamedSoft.Template.Infrastructure.Persistence.Seed;

public static class IdentitySeedExtensions
{
    public static async Task SeedIdentityAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        await IdentitySeeder.SeedAsync(context, roleManager);
    }
}