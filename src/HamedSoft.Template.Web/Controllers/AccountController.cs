using HamedSoft.Template.Application.Features.Commands.Auth.Login;
using HamedSoft.Template.Application.Features.Commands.Auth.Register;
using HamedSoft.Template.Web.Security;
using HamedSoft.Template.Web.ViewModels.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HamedSoft.Template.Web.Controllers;

public class AccountController : Controller
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
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

        var command = new LoginCommand(
            model.UserName,
            model.Password);

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

        foreach (var permission in loginResult.Permissions)
        {
            claims.Add(new Claim(CustomClaimTypes.Permission, permission));
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
}