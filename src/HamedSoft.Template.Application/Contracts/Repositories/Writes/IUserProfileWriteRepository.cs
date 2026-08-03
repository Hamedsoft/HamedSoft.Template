using HamedSoft.Template.Domain.UserProfiles;

namespace HamedSoft.Template.Application.Contracts.Repositories.Writes;

public interface IUserProfileWriteRepository
{
    Task AddAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default);
}