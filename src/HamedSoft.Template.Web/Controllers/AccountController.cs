using HamedSoft.Template.Application.Features.Commands.Auth.Login;
using HamedSoft.Template.Application.Features.Commands.Auth.Register;
using HamedSoft.Template.Web.ViewModels.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        // فعلاً اینجا نگه می‌داریم

        return RedirectToAction("Index", "Home");
    }
}