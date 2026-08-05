using HamedSoft.Template.Application.Contracts.Permissions;
using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Services;

internal sealed class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _context;

    public PermissionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyList<PermissionDto>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var permissions = await _context.Permissions
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new PermissionDto(
                x.Id,
                x.Name))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<PermissionDto>>
            .Success(permissions);
    }
}