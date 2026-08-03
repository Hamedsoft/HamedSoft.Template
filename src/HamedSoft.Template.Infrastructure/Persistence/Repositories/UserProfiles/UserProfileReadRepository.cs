using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Domain.UserProfiles;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Persistence.Repositories.UserProfiles;

internal sealed class UserProfileReadRepository
    : IUserProfileReadRepository
{
    private readonly ApplicationDbContext _context;

    public UserProfileReadRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetByIdAsync(
        UserProfileId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserProfiles
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}