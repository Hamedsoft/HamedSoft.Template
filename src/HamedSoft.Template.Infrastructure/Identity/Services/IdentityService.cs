using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Services;

public sealed class IdentityService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _context;


    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Result<AuthenticatedUser>> LoginAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user is null)
            return Result<AuthenticatedUser>.Failure("نام کاربری یا رمز عبور اشتباه است.");

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
            return Result<AuthenticatedUser>.Failure("نام کاربری یا رمز عبور اشتباه است.");

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = await( from role in _context.Roles 
                                 join rolePermission in _context.RolePermissions on role.Id equals rolePermission.RoleId 
                                 join permission in _context.Permissions on rolePermission.PermissionId equals permission.Id
                                 where roles.Contains(role.Name!)
                                 select permission.Name
                                ).Distinct().ToListAsync(cancellationToken);
        return Result<AuthenticatedUser>.Success(new AuthenticatedUser( user.Id, user.UserName ?? string.Empty, user.UserName ?? string.Empty, roles.ToArray(), permissions));
    }

    public async Task<Result<RegisteredUser>> RegisterAsync(
        string userName,
        string password,
        CancellationToken cancellationToken = default)
    {
        var exists = await _userManager.FindByNameAsync(userName);

        if (exists is not null)
            return Result<RegisteredUser>.Failure("نام کاربری قبلاً ثبت شده است.");

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = userName
        };

        var createResult = await _userManager.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(
                Environment.NewLine,
                createResult.Errors.Select(x => x.Description));

            return Result<RegisteredUser>.Failure(errors);
        }

        return Result<RegisteredUser>.Success(
            new RegisteredUser(user.Id));
    }

    public async Task<Result> LogoutAsync(CancellationToken cancellationToken = default)
    {
        await _signInManager.SignOutAsync();

        return Result.Success();
    }
    public async Task<Result> ChangePasswordAsync(UserId userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());

        if (user is null)
            return Result.Failure("کاربر یافت نشد.");

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!result.Succeeded)
        {
            var error = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
            return Result.Failure(error);
        }

        return Result.Success();
    }
}