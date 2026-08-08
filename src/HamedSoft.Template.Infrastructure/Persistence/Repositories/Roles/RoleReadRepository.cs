using System.Linq;
using HamedSoft.Template.Application.Common.Models;
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
            .OrderBy(x => x.Name)
            .Select(role => new RoleDto(
                role.Id,
                role.Name!))
            .ToListAsync(cancellationToken);
    }


    public async Task<RolePermissionsDto?> GetByIdAsync(
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
            .Select(permission => new LookupItemDto(
                permission.Id,
                permission.Name,
                _context.RolePermissions.Any(
                    x =>
                        x.RoleId == roleId &&
                        x.PermissionId == permission.Id)))
            .ToListAsync(cancellationToken);


        return new RolePermissionsDto(
            role.Id,
            role.Name!,
            permissions);
    }
}