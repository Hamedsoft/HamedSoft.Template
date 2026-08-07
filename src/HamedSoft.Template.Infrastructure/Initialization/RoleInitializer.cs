using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Identity.Seed;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace HamedSoft.Template.Infrastructure.Initialization;

public sealed class RoleInitializer : IInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleInitializer(
        ApplicationDbContext context,
        RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync(
        CancellationToken cancellationToken = default)
    {
        await RoleSeeder.SeedAsync(
            _context,
            _roleManager);
    }
}