using System.Security.Claims;
using HamedSoft.Template.Application.Contracts.Services;
using HamedSoft.Template.Application.Features.Commands.Auth.ChangePassword;
using HamedSoft.Template.Application.Features.Commands.Auth.Login;
using HamedSoft.Template.Application.Features.Commands.Auth.Register;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Web.Security;
using HamedSoft.Template.Web.ViewModels.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers;

public class AccountController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public AccountController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var command = new RegisterCommand(model.UserName ,model.Password, model.FirstName, model.LastName);
        var result = await _mediator.Send(command);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(model);
        }

        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var command = new LoginCommand(model.UserName, model.Password);

        var result = await _mediator.Send(command);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(model);
        }

        var loginResult = result.Value!;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, loginResult.UserId.ToString()),
            new(ClaimTypes.Name, loginResult.UserName)
        };


        if (loginResult.Roles.Contains(SystemRoles.Admin))
        {
            claims.Add(new Claim(CustomClaimTypes.Permission, SystemPermissions.All));
        }
        else
        {
            if (loginResult.Roles.Any(x => x.Equals( SystemRoles.Admin, StringComparison.OrdinalIgnoreCase)))
            {
                claims.Add(new Claim( CustomClaimTypes.Permission, SystemPermissions.All));
            }
            else
            {
                foreach (var permission in loginResult.Permissions)
                {
                    claims.Add(new Claim(CustomClaimTypes.Permission, permission));
                }
            }
        }


        var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);


        return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (_currentUser.UserId is null)
            return Unauthorized();

        var command = new ChangePasswordCommand(
            _currentUser.UserId.Value,
            model.CurrentPassword,
            model.NewPassword);

        var result = await _mediator.Send(command);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(model);
        }

        TempData["Success"] = "رمز عبور با موفقیت تغییر کرد.";

        return RedirectToAction("Index", "Home");
    }
}