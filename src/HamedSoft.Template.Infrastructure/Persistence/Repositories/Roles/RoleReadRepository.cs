using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HamedSoft.Template.Infrastructure.Repositories.Roles;

internal sealed class RoleReadRepository : IRoleReadRepository
{
    private readonly ApplicationDbContext _context;

    public RoleReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AsNoTracking()
            .AnyAsync(
                x => x.Id == roleId,
                cancellationToken);
    }

    public async Task<RolePermissionsDto?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
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
            .Select(permission => new RolePermissionItemDto(
                permission.Id,
                permission.Name,
                permission.Module,
                permission.Category,
                permission.DisplayName,
                permission.Description,
                _context.RolePermissions.Any(
                    x =>
                        x.RoleId == roleId &&
                        x.PermissionId == permission.Id)))
            .ToListAsync(cancellationToken);

        return new RolePermissionsDto(
            role.Id,
            role.Name!,
             role.Name! == SystemRoles.Admin,
            permissions);
    }

    async Task<IReadOnlyList<RoleDto>> IRoleReadRepository.GetAllAsync(bool withAdmin, CancellationToken cancellationToken)
    {
        var result = await _context.Roles
         .AsNoTracking()
         .Select(role => new RoleDto(
             role.Id,
             role.Name!,
             role.Name! == SystemRoles.Admin))
         .ToListAsync(cancellationToken);
        return result.Where(m => m.IsAdmin == withAdmin).ToList();
    }
    public async Task<bool> IsAdminAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.Id == roleId &&
                    x.Name == SystemRoles.Admin,
                cancellationToken);
    }

}