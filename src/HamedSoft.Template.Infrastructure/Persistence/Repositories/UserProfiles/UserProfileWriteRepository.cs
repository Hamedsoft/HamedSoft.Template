using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Domain.UserProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HamedSoft.Template.Infrastructure.Persistence.Repositories.UserProfiles;

internal sealed class UserProfileWriteRepository : IUserProfileWriteRepository
{
    private readonly DbSet<UserProfile> _userProfiles;

    public UserProfileWriteRepository(ApplicationDbContext dbContext)
    {
        _userProfiles = dbContext.UserProfiles;
    }
    public async Task<UserProfile?> GetUserProfileByIdAsync(UserProfileId userProfileId,CancellationToken cancellation)
    {
        return await _userProfiles.Where(m => m.Id == userProfileId).FirstOrDefaultAsync(cancellation);
    }
    public void Update(UserProfile userProfile)
    {
        _userProfiles.Update(userProfile);
    }
    public async Task<EntityEntry<UserProfile>> AddAsync(UserProfile userProfile,CancellationToken cancellation)
    {
      return await _userProfiles.AddAsync(userProfile,cancellation);
    }


}
