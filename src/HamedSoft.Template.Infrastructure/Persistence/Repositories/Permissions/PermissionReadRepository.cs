using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Repositories.Permissions;

internal sealed class PermissionReadRepository
    : IPermissionReadRepository
{
    private readonly ApplicationDbContext _context;

    public PermissionReadRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AllExistAsync(
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default)
    {
        if (permissionIds.Count == 0)
            return true;

        var distinctIds = permissionIds
            .Distinct()
            .ToList();

        var existingCount = await _context.Permissions
            .AsNoTracking()
            .CountAsync(
                x => distinctIds.Contains(x.Id),
                cancellationToken);

        return existingCount == distinctIds.Count;
    }
}