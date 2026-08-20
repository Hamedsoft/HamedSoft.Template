using HamedSoft.Template.Infrastructure.Identity.Options;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.UnitOfWork;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Domain.UserProfiles;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Identity.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HamedSoft.Template.Infrastructure.Initialization;

public sealed class SystemUserInitializer : IInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOptions<AdminUserOptions> _adminOptions;
    private readonly IOptions<ManagerUserOptions> _managerOptions;
    private readonly IUserProfileWriteRepository _userProfileWriteRepository;
    private readonly IApplicationUnitOfWork _unitOfWork;

    public SystemUserInitializer(UserManager<ApplicationUser> userManager, IOptions<AdminUserOptions> adminOptions, IOptions<ManagerUserOptions> managerOptions,
    IUserProfileWriteRepository userProfileWriteRepository, IApplicationUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _adminOptions = adminOptions;
        _managerOptions = managerOptions;
        _userProfileWriteRepository = userProfileWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var seededUsers = await SystemUserSeeder.SeedAsync(_userManager, _adminOptions, _managerOptions);
        foreach (var seededUser in seededUsers)
        {
            if (!seededUser.IsCreated)
                continue;

            var profile = UserProfile.Create(UserProfileId.Create(seededUser.User.Id), string.Empty, string.Empty);

            await _userProfileWriteRepository.AddAsync(profile, cancellationToken);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}