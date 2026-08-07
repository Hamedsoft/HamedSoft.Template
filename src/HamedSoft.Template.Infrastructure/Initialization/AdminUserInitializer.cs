using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Identity.Options;
using HamedSoft.Template.Infrastructure.Identity.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HamedSoft.Template.Infrastructure.Initialization;

public sealed class AdminUserInitializer : IInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOptions<AdminUserOptions> _options;

    public AdminUserInitializer(
        UserManager<ApplicationUser> userManager,
        IOptions<AdminUserOptions> options)
    {
        _userManager = userManager;
        _options = options;
    }

    public async Task InitializeAsync(
        CancellationToken cancellationToken = default)
    {
        await AdminUserSeeder.SeedAsync(
            _userManager,
            _options);
    }
}