using HamedSoft.Template.Application.Contracts.Security;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Security;

internal sealed class PermissionChecker : IPermissionChecker
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public PermissionChecker(
        ApplicationDbContext context,
        ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<bool> HasPermissionAsync(
        string permissionName,
        CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsAuthenticated ||
            !_currentUser.UserId.HasValue)
        {
            return false;
        }

        return await _context.UserRoles
            .Where(x => x.UserId == _currentUser.UserId.Value)
            .Join(
                _context.RolePermissions,
                userRole => userRole.RoleId,
                rolePermission => rolePermission.RoleId,
                (userRole, rolePermission) => rolePermission.PermissionId)
            .Join(
                _context.Permissions,
                permissionId => permissionId,
                permission => permission.Id,
                (permissionId, permission) => permission.Name)
            .AnyAsync(
                name => name == permissionName,
                cancellationToken);
    }
}