using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Infrastructure.Identity.Seed;
using HamedSoft.Template.Infrastructure.Persistence;

namespace HamedSoft.Template.Infrastructure.Initialization;

public sealed class SystemPermissionInitializer : IInitializer
{
    private readonly ApplicationDbContext _context;

    public SystemPermissionInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await SystemPermissionSeeder.SeedAsync(_context, SystemRoles.Admin);
        await SystemPermissionSeeder.SeedAsync(_context, SystemRoles.Manager);
    }
}