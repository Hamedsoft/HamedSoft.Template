using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Features.Commands.Roles.AssignPermissions;
using HamedSoft.Template.Web.ViewModels.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers;

public class RolesController : Controller
{
    private readonly IRoleManagementService _roleManagementService;
    private readonly IMediator _mediator;

    public RolesController(
        IRoleManagementService roleManagementService,
        IMediator mediator)
    {
        _roleManagementService = roleManagementService;
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var result = await _roleManagementService
            .GetAllAsync(cancellationToken);


        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;

            return View(Array.Empty<RoleDto>());
        }


        return View(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Permissions(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _roleManagementService
            .GetByIdAsync(
                id,
                cancellationToken);


        if (!result.Succeeded)
            return NotFound();


        var model = new RolePermissionViewModel
        {
            RoleId = result.Value.RoleId,
            RoleName = result.Value.RoleName,

            Permissions = result.Value.Permissions
                .Select(x => new PermissionItemViewModel
                {
                    PermissionId = x.PermissionId,
                    Name = x.PermissionName,
                    IsAssigned = x.IsAssigned
                })
                .ToList()
        };

        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Permissions(
    RolePermissionViewModel model,
    CancellationToken cancellationToken)
    {
        var command = new AssignPermissionsCommand(
            model.RoleId,
            model.PermissionIds);


        var result = await _mediator.Send(
            command,
            cancellationToken);


        if (!result.Succeeded)
        {
            ModelState.AddModelError(
                string.Empty,
                result.Error!);

            return View(model);
        }


        return RedirectToAction(
            nameof(Permissions),
            new
            {
                id = model.RoleId
            });
    }
}