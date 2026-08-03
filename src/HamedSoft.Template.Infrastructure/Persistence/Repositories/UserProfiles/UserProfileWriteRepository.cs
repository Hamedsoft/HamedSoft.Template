using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Domain.UserProfiles;

namespace HamedSoft.Template.Infrastructure.Persistence.Repositories.UserProfiles;

internal sealed class UserProfileWriteRepository
    : IUserProfileWriteRepository
{
    private readonly ApplicationDbContext _context;

    public UserProfileWriteRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default)
    {
        await _context.UserProfiles.AddAsync(profile, cancellationToken);
    }

    public async Task UpdateAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default)
    {
        _context.UserProfiles.Update(profile);

        await Task.CompletedTask;
    }

    public async Task RemoveAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default)
    {
        _context.UserProfiles.Remove(profile);

        await Task.CompletedTask;
    }
}