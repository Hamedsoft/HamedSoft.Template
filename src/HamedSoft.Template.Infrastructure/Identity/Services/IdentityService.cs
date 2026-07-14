using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.SharedKernel.Common;
using Microsoft.AspNetCore.Identity;

namespace HamedSoft.Template.Infrastructure.Identity.Services;

public sealed class IdentityService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
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

        return Result<AuthenticatedUser>.Success(new AuthenticatedUser(user.Id, user.UserName ?? string.Empty, user.UserName ?? string.Empty, roles.ToArray()));
    }

    public Task<Result<RegisteredUser>> RegisterAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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