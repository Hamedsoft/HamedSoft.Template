using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Repositories.Users;

internal sealed class UserReadRepository : IUserReadRepository
{
    private readonly ApplicationDbContext _context;

    public UserReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<UserListItem>> GetPagedAsync(bool withAdminUser, PageRequest pageRequest, string? search = null, UserStatus? status = null, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var query = from user in _context.Users.AsNoTracking()
                    join profile in _context.UserProfiles.AsNoTracking() on user.Id equals profile.Id
                    select new
                    {
                        User = user,
                        Profile = profile
                    };

        if (!withAdminUser)
            query = query.Where(x => x.User.UserName != SystemRoles.Admin);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(x => (x.User.UserName != null && x.User.UserName.Contains(search)) ||
                                     (x.User.Email != null && x.User.Email.Contains(search)) ||
                                     (x.User.PhoneNumber != null && x.User.PhoneNumber.Contains(search)) ||
                                     (x.Profile.FirstName.Contains(search) || x.Profile.LastName.Contains(search)));
        }

        query = status switch
        {
            UserStatus.Active => query.Where(x => x.User.IsActive && (!x.User.LockoutEnd.HasValue || x.User.LockoutEnd <= now)),
            UserStatus.Inactive => query.Where(x => !x.User.IsActive && (!x.User.LockoutEnd.HasValue || x.User.LockoutEnd <= now)),
            UserStatus.Locked => query.Where(x => x.User.LockoutEnd.HasValue && x.User.LockoutEnd > now),
            _ => query
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query.OrderBy(x => x.User.UserName).Skip(pageRequest.Skip).Take(pageRequest.NormalizedPageSize)
            .Select(x => new
            {
                x.User.Id,
                UserName = x.User.UserName ?? string.Empty,
                x.User.IsActive,
                x.User.LockoutEnd,
                x.User.Email,
                x.User.PhoneNumber,
                x.Profile.FirstName,
                x.Profile.LastName
            })
            .ToListAsync(cancellationToken);

        var userIds = users.Select(x => x.Id).ToArray();

        var roles = await (from userRole in _context.UserRoles.AsNoTracking()
                           join role in _context.Roles.AsNoTracking() on userRole.RoleId equals role.Id
                           where userIds.Contains(userRole.UserId)
                           select new
                           {
                               userRole.UserId,
                               RoleName = role.Name!
                           })
                           .ToListAsync(cancellationToken);

        var rolesByUser = roles.GroupBy(x => x.UserId).ToDictionary(x => x.Key, x => x.Select(r => r.RoleName).ToArray());

        var items = users
            .Select(user =>
            {
                rolesByUser.TryGetValue(user.Id, out var userRoles);

                var isLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > now;

                var profile = new UserProfileDto(user.Id, user.UserName, user.FirstName, user.LastName, user.Email, user.PhoneNumber);

                return new UserListItem(user.Id, user.UserName, user.UserName, user.IsActive, isLocked, profile, userRoles ?? Array.Empty<string>());
            }).ToList();

        return new PagedResult<UserListItem>(items, pageRequest.NormalizedPageNumber, pageRequest.NormalizedPageSize, totalCount);
    }
}