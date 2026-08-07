using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Features.Commands.Users.AssignRoles;
using HamedSoft.Template.Application.Features.Queries.Users.GetUserRoles;
using HamedSoft.Template.Application.Features.Queries.Users.GetUsers;
using HamedSoft.Template.Web.ViewModels.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers;

public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetUsersQuery(),
            cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
            return View(Array.Empty<UserListItem>());
        }

        return View(result.Value);
    }
    [HttpGet]
    public async Task<IActionResult> Roles(
    Guid userId,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetUserRolesQuery(userId),
            cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
            return RedirectToAction(nameof(Index));
        }

        var model = new UserRolesViewModel
        {
            UserId = result.Value!.UserId,
            UserName = result.Value.UserName,

            Roles = result.Value.Roles
                .Select(x => new UserRoleItemViewModel
                {
                    RoleId = x.Id,
                    RoleName = x.Name,
                    Selected = x.Selected
                })
                .ToList()
        };

        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Roles(
        UserRolesViewModel model,
        CancellationToken cancellationToken)
    {
        var selectedRoleIds = model.Roles
            .Where(x => x.Selected)
            .Select(x => x.RoleId)
            .ToList();

        var result = await _mediator.Send(
            new AssignRolesCommand(
                model.UserId,
                selectedRoleIds),
            cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
            return View(model);
        }

        TempData["Success"] = "نقش‌های کاربر با موفقیت بروزرسانی شد.";

        return RedirectToAction(nameof(Index));
    }
}