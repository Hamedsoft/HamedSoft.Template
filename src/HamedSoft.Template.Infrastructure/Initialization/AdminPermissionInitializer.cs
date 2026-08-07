using HamedSoft.Template.Infrastructure.Identity.Seed;
using HamedSoft.Template.Infrastructure.Persistence;

namespace HamedSoft.Template.Infrastructure.Initialization;

public sealed class AdminPermissionInitializer
    : IInitializer
{
    private readonly ApplicationDbContext _context;

    public AdminPermissionInitializer(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync(
        CancellationToken cancellationToken = default)
    {
        await AdminRolePermissionSeeder.SeedAsync(
            _context);
    }
}