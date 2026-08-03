using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Domain.UserProfiles;

namespace HamedSoft.Template.Application.Contracts.Repositories.Reads;

public interface IUserProfileReadRepository
{
    Task<UserProfile?> GetByIdAsync(
        UserProfileId id,
        CancellationToken cancellationToken = default);
}