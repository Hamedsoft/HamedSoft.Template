using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Repositories.Roles;

internal sealed class RoleReadRepository : IRoleReadRepository
{
    private readonly ApplicationDbContext _context;

    public RoleReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<bool> ExistsAsync(
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AsNoTracking()
            .AnyAsync(
                x => x.Id == roleId,
                cancellationToken);
    }


    public async Task<IReadOnlyList<RoleDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AsNoTracking()
            .Select(role => new RoleDto(
                role.Id,
                role.Name!,
                new List<RolePermissionDto>()))
            .ToListAsync(cancellationToken);
    }


    public async Task<RoleDto?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == roleId,
                cancellationToken);


        if (role is null)
            return null;


        var permissions = await _context.Permissions
            .AsNoTracking()
            .Select(permission => new RolePermissionDto(
                permission.Id,
                permission.Name,
                _context.RolePermissions.Any(
                    x =>
                        x.RoleId == roleId &&
                        x.PermissionId == permission.Id)))
            .ToListAsync(cancellationToken);


        return new RoleDto(
            role.Id,
            role.Name!,
            permissions);
    }
}