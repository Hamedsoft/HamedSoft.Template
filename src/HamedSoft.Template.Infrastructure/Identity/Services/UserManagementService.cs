using HamedSoft.Template.Application.Common.Models;
using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Services;

public sealed class UserManagementService : IUserManagementService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserManagementService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<IReadOnlyList<UserListItem>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var users = await _userManager.Users
            .OrderBy(x => x.UserName)
            .ToListAsync(cancellationToken);

        var result = new List<UserListItem>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(
                new UserListItem(
                    user.Id,
                    user.UserName ?? string.Empty,
                    user.UserName ?? string.Empty,
                    roles.ToArray()));
        }

        return Result<IReadOnlyList<UserListItem>>
            .Success(result);
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
            roles
                .Select(r =>
                    new LookupItemDto(
                        r.Id,
                        r.Name!,
                        userRoles.Contains(r.Name!)))
                .ToList());

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
            .Where(x => roleIds.Contains(x.Id))
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
}