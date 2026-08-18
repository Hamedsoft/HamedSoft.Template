using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Features.Commands.Roles.AssignPermissions;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Web.Security;
using HamedSoft.Template.Web.ViewModels.Common.Pagination;
using HamedSoft.Template.Web.ViewModels.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers;

public class RolesController : Controller
{
    private readonly IRoleManagementService _roleManagementService;
    private readonly IMediator _mediator;

    public RolesController(IRoleManagementService roleManagementService, IMediator mediator)
    {
        _roleManagementService = roleManagementService;
        _mediator = mediator;
    }

    [HttpGet]
    [Permission(PermissionConstants.Roles.View)]
    public async Task<IActionResult> Index(
    int pageNumber = 1,
    int pageSize = 10,
    string? search = null,
    CancellationToken cancellationToken = default)
    {
        pageSize = Math.Clamp(pageSize, 1, 100);

        var result = await _roleManagementService.GetPagedAsync(
            false,
            pageNumber,
            pageSize,
            search,
            cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;

            return View(new RolesIndexViewModel());
        }

        var pagedRoles = result.Value!;

        var model = new RolesIndexViewModel
        {
            Roles = pagedRoles,

            Search = search,

            PageSize = pageSize,

            Pagination = new PaginationViewModel
            {
                PageNumber = pagedRoles.PageNumber,
                PageSize = pagedRoles.PageSize,
                TotalCount = pagedRoles.TotalCount,
                TotalPages = pagedRoles.TotalPages,
                Action = nameof(Index),
                Controller = "Roles",
                Search = search
            }
        };

        return View(model);
    }

    [HttpGet]
    [Permission(PermissionConstants.Roles.Create)]
    public IActionResult Create()
    {
        return View(new CreateRoleViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Roles.Create)]
    public async Task<IActionResult> Create(CreateRoleViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _roleManagementService.CreateAsync(
            model.RoleName,
            cancellationToken);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(
                string.Empty,
                result.Error ?? "خطا در ایجاد نقش.");

            return View(model);
        }

        TempData["Success"] = "نقش با موفقیت ایجاد شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Permission(PermissionConstants.Roles.Edit)]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleManagementService.GetByIdAsync(
            id,
            cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }

        var model = new EditRoleViewModel
        {
            RoleId = result.Value!.RoleId,
            RoleName = result.Value.RoleName
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Roles.Edit)]
    public async Task<IActionResult> Edit(EditRoleViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _roleManagementService.UpdateAsync(
            model.RoleId,
            model.RoleName,
            cancellationToken);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(
                string.Empty,
                result.Error ?? "خطا در بروزرسانی نقش.");

            return View(model);
        }

        TempData["Success"] = "نقش با موفقیت بروزرسانی شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Roles.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleManagementService.DeleteAsync(
            id,
            cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "نقش با موفقیت حذف شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Permission(PermissionConstants.Roles.AssignPermissions)]
    public async Task<IActionResult> Permissions(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleManagementService
            .GetByIdAsync(
                id,
                cancellationToken);

        if (!result.Succeeded)
            return NotFound();

        var model = new RolePermissionViewModel
        {
            RoleId = result.Value!.RoleId,
            RoleName = result.Value.RoleName,
            IsAdmin = result.Value.IsAdmin,
            Permissions = result.Value.Permissions
                .Select(x => new PermissionItemViewModel
                {
                    PermissionId = x.Id,
                    Name = x.Name,
                    Module = x.Module,
                    Category = x.Category,
                    DisplayName = x.DisplayName,
                    Description = x.Description,
                    IsAssigned = x.Selected
                })
                .ToList()
        };

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Roles.AssignPermissions)]
    public async Task<IActionResult> Permissions(RolePermissionViewModel model, CancellationToken cancellationToken)
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
            nameof(Index));
    }
}