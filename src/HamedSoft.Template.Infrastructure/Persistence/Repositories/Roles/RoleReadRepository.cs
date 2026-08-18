using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Security;
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

    public async Task<bool> ExistsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AsNoTracking()
            .AnyAsync(x => x.Id == roleId, cancellationToken);
    }

    public async Task<bool> IsAdminAsync(
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AsNoTracking()
            .AnyAsync(x => x.Id == roleId && x.Name == SystemRoles.Admin, cancellationToken);
    }

    public async Task<RolePermissionsDto?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);

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
                _context.RolePermissions.Any(x => x.RoleId == roleId && x.PermissionId == permission.Id)))
            .ToListAsync(cancellationToken);

        return new RolePermissionsDto(role.Id, role.Name!, role.Name! == SystemRoles.Admin, permissions);
    }

    public async Task<PagedResult<RoleDto>> GetPagedAsync(
    bool includeAdmin,
    int pageNumber,
    int pageSize,
    string? search = null,
    CancellationToken cancellationToken = default)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _context.Roles
            .AsNoTracking();

        if (!includeAdmin)
        {
            query = query.Where(
                role => role.Name != SystemRoles.Admin);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(
                role => role.Name!.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(role => role.Name)
            .ThenBy(role => role.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(role => new RoleDto(
                role.Id,
                role.Name!,
                role.Name! == SystemRoles.Admin))
            .ToListAsync(cancellationToken);

        return new PagedResult<RoleDto>(
            items,
            pageNumber,
            pageSize,
            totalCount);
    }
}