using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Domain.UserProfiles;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HamedSoft.Template.Application.Contracts.Repositories.Writes;

public interface IUserProfileWriteRepository
{
    Task<EntityEntry<UserProfile>> AddAsync(UserProfile userProfile, CancellationToken cancellation);
    Task<UserProfile?> GetUserProfileByIdAsync(UserProfileId userProfileId,CancellationToken cancellation);
    void Update(UserProfile userProfile);
}
