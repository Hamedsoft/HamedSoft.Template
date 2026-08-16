using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Contracts.UnitOfWork;
using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HamedSoft.Template.Infrastructure.Identity.Services;

public sealed class UserManagementService : IUserManagementService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUserProfileReadRepository _userProfileReadRepository;
    private readonly IUserProfileWriteRepository _userProfileWriteRepository;
    private readonly IApplicationUnitOfWork _unitOfWork;
    private readonly IUserReadRepository _userReadRepository;
    private readonly ApplicationDbContext _context;

    public UserManagementService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IUserProfileReadRepository userProfileReadRepository,
        IUserProfileWriteRepository userProfileWriteRepository,
        IApplicationUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userProfileReadRepository = userProfileReadRepository;
        _userProfileWriteRepository = userProfileWriteRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result<PagedResult<UserListItem>>> GetAllAsync(
    bool withAdminUser,
    int pageNumber,
    int pageSize,
    string? search = null,
    UserStatus? status = null,
    CancellationToken cancellationToken = default)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        IQueryable<ApplicationUser> query =
            _userManager.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(user =>
                user.UserName!.Contains(search) ||
                user.Email!.Contains(search) ||
                user.PhoneNumber!.Contains(search) ||
                _context.UserProfiles.Any(profile =>
                    profile.Id == user.Id &&
                    (
                        profile.FirstName.Contains(search) ||
                        profile.LastName.Contains(search)
                    )));
        }

        if (!withAdminUser)
        {
            query = query.Where(
                x => x.UserName != SystemRoles.Admin);
        }

        var now = DateTimeOffset.UtcNow;

        if (status.HasValue)
        {
            query = status.Value switch
            {
                UserStatus.Active =>
                    query.Where(x =>
                        x.IsActive &&
                        (!x.LockoutEnd.HasValue ||
                         x.LockoutEnd.Value <= now)),

                UserStatus.Inactive =>
                    query.Where(x =>
                        !x.IsActive),

                UserStatus.Locked =>
                    query.Where(x =>
                        x.LockoutEnd.HasValue &&
                        x.LockoutEnd.Value > now),

                _ => query
            };
        }

        var totalCount = await query
            .CountAsync(cancellationToken);

        var users = await query
            .OrderBy(x => x.UserName)
            .ThenBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        if (users.Count == 0)
        {
            return Result<PagedResult<UserListItem>>.Success(
                new PagedResult<UserListItem>(
    Array.Empty<UserListItem>(),
    pageNumber,
    pageSize,
    totalCount,
    search));
        }

        var userIds = users
            .Select(x => x.Id)
            .ToArray();

        var profileIds = userIds
            .Select(UserProfileId.Create)
            .ToArray();

        var profiles = await _userProfileReadRepository
            .GetByIdsAsync(
                profileIds,
                cancellationToken);

        var profileDictionary = profiles
            .ToDictionary(x => x.Id);

        var userRoles = await (
            from userRole in _context.UserRoles.AsNoTracking()
            join role in _context.Roles.AsNoTracking()
                on userRole.RoleId equals role.Id
            where userIds.Contains(userRole.UserId)
            select new
            {
                userRole.UserId,
                RoleName = role.Name!
            })
            .ToListAsync(cancellationToken);

        var rolesByUser = userRoles
            .GroupBy(x => x.UserId)
            .ToDictionary(
                x => x.Key,
                x => x
                    .Select(r => r.RoleName)
                    .OrderBy(r => r)
                    .ToArray());

        var result = new List<UserListItem>(
            users.Count);

        foreach (var user in users)
        {
            profileDictionary.TryGetValue(
                UserProfileId.Create(user.Id),
                out var profile);

            rolesByUser.TryGetValue(
                user.Id,
                out var roles);

            var isLocked =
                user.LockoutEnd.HasValue &&
                user.LockoutEnd.Value > now;

            var userProfile = new UserProfileDto(
                user.Id,
                user.UserName ?? string.Empty,
                profile?.FirstName ?? string.Empty,
                profile?.LastName ?? string.Empty,
                user.Email,
                user.PhoneNumber);

            result.Add(
                new UserListItem(
                    user.Id,
                    user.UserName ?? string.Empty,
                    user.UserName ?? string.Empty,
                    user.IsActive,
                    isLocked,
                    userProfile,
                    roles ?? Array.Empty<string>()));
        }

        return Result<PagedResult<UserListItem>>.Success(
            new PagedResult<UserListItem>(
    result,
    pageNumber,
    pageSize,
    totalCount,
    search));
    }

    public async Task<Result<UserRolesDto>> GetRolesAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);

        if (user is null)
            return Result<UserRolesDto>.Failure("کاربر یافت نشد.");

        var userRoles = await _userManager.GetRolesAsync(user);

        var roles = await _roleManager.Roles
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        var dto = new UserRolesDto(
            user.Id,
            user.UserName ?? string.Empty,
            roles.Select(r => new SelectRole(new RoleDto(r.Id, r.Name!, r.Name! == SystemRoles.Admin), userRoles.Contains(r.Name!)))
            .Where(m => !m.roleDto.IsAdmin).ToList());
        return Result<UserRolesDto>.Success(dto);
    }

    public async Task<Result> AssignRolesAsync(
        Guid userId,
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure("کاربر یافت نشد.");

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Count > 0)
        {
            await _userManager.RemoveFromRolesAsync(
                user,
                currentRoles);
        }

        var selectedRoleNames = await _roleManager.Roles
            .Where(x => roleIds.Contains(x.Id))
            .Select(x => x.Name!)
            .ToListAsync(cancellationToken);

        var roles = await _roleManager.Roles
            .Where(x => roleIds.Contains(x.Id) && x.Name! != SystemRoles.Admin)
            .Select(x => x.Name!)
            .ToListAsync(cancellationToken);


        var addResult = await _userManager.AddToRolesAsync(
            user,
            roles);

        if (!addResult.Succeeded)
        {
            return Result.Failure(
                addResult.Errors.First().Description);
        }

        return Result.Success();
    }
    public async Task<Result<UserProfileDto>> GetProfileAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);

        if (user is null)
            return Result<UserProfileDto>.Failure("کاربر یافت نشد.");

        var profile = await _userProfileReadRepository.GetByIdAsync(
            UserProfileId.Create(userId),
            cancellationToken);

        if (profile is null)
            return Result<UserProfileDto>.Failure("پروفایل کاربر یافت نشد.");

        var dto = new UserProfileDto(
            user.Id,
            user.UserName ?? string.Empty,
            profile.FirstName,
            profile.LastName,
            user.Email,
            user.PhoneNumber);

        return Result<UserProfileDto>.Success(dto);
    }
    public async Task<Result> UpdateProfileAsync(
    UserProfileDto profileDto,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(
                x => x.Id == profileDto.UserId,
                cancellationToken);

        if (user is null)
            return Result.Failure("کاربر یافت نشد.");


        var profile = await _userProfileReadRepository.GetByIdAsync(
            UserProfileId.Create(profileDto.UserId),
            cancellationToken);


        if (profile is null)
            return Result.Failure("پروفایل کاربر یافت نشد.");


        profile.UpdateName(
            profileDto.FirstName,
            profileDto.LastName);


        await _userProfileWriteRepository.UpdateAsync(
            profile,
            cancellationToken);


        user.Email = profileDto.Email;
        user.PhoneNumber = profileDto.PhoneNumber;


        var identityResult = await _userManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
        {
            return Result.Failure(
                identityResult.Errors.First().Description);
        }


        await _unitOfWork.SaveChangesAsync(cancellationToken);


        return Result.Success();
    }
    public async Task<Result<UserSecurityDto>> GetSecurityAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);


        if (user is null)
        {
            return Result<UserSecurityDto>.Failure(
                "کاربر یافت نشد.");
        }


        var dto = new UserSecurityDto(
            user.Id,
            user.UserName ?? string.Empty,
            user.LockoutEnd.HasValue &&
            user.LockoutEnd.Value > DateTimeOffset.UtcNow,
            user.IsActive);


        return Result<UserSecurityDto>.Success(dto);
    }
    public async Task<Result> ResetPasswordAsync(
    Guid userId,
    string newPassword,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);


        if (user is null)
            return Result.Failure("کاربر یافت نشد.");


        var token = await _userManager
            .GeneratePasswordResetTokenAsync(user);


        var result = await _userManager
            .ResetPasswordAsync(
                user,
                token,
                newPassword);


        if (!result.Succeeded)
        {
            return Result.Failure(
                result.Errors.First().Description);
        }


        return Result.Success();
    }
    public async Task<Result> UpdateStatusAsync(
    Guid userId,
    bool isActive,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);


        if (user is null)
            return Result.Failure("کاربر یافت نشد.");


        user.IsActive = isActive;


        var result = await _userManager.UpdateAsync(user);


        if (!result.Succeeded)
        {
            return Result.Failure(
                result.Errors.First().Description);
        }


        return Result.Success();
    }
    public async Task<Result> LockAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);


        if (user is null)
            return Result.Failure("کاربر یافت نشد.");


        user.LockoutEnabled = true;


        var result = await _userManager
            .SetLockoutEndDateAsync(
                user,
                DateTimeOffset.UtcNow.AddYears(100));


        if (!result.Succeeded)
        {
            return Result.Failure(
                result.Errors.First().Description);
        }


        return Result.Success();
    }
    public async Task<Result> UnlockAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);


        if (user is null)
            return Result.Failure("کاربر یافت نشد.");


        var result = await _userManager
            .SetLockoutEndDateAsync(
                user,
                null);


        if (!result.Succeeded)
        {
            return Result.Failure(
                result.Errors.First().Description);
        }


        return Result.Success();
    }
}