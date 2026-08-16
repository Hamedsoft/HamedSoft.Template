using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Contracts.Security;
using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Features.Commands.Users.AssignRoles;
using HamedSoft.Template.Application.Features.Commands.Users.LockUser;
using HamedSoft.Template.Application.Features.Commands.Users.ResetPassword;
using HamedSoft.Template.Application.Features.Commands.Users.UnlockUser;
using HamedSoft.Template.Application.Features.Commands.Users.UpdateProfile;
using HamedSoft.Template.Application.Features.Commands.Users.UpdateUserStatus;
using HamedSoft.Template.Application.Features.Queries.Users.GetUserProfile;
using HamedSoft.Template.Application.Features.Queries.Users.GetUserRoles;
using HamedSoft.Template.Application.Features.Queries.Users.GetUsers;
using HamedSoft.Template.Application.Features.Queries.Users.GetUserSecurity;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Web.Security;
using HamedSoft.Template.Web.ViewModels.Common.Pagination;
using HamedSoft.Template.Web.ViewModels.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.Controllers;

public class UsersController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public UsersController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    [Permission(PermissionConstants.Users.View)]
    public async Task<IActionResult> Index(
    int activePageNumber = 1,
    int inactivePageNumber = 1,
    int lockedPageNumber = 1,
    int pageSize = 10,
    string? search = null,
    string activeTab = "active",
    CancellationToken cancellationToken = default)
    {
        pageSize = Math.Clamp(pageSize, 1, 100);

        activeTab = activeTab.ToLowerInvariant();

        if (activeTab is not ("active" or "deactive" or "locked"))
        {
            activeTab = "active";
        }

        var activeResult = await _mediator.Send(
            new GetUsersQuery(
                activePageNumber,
                pageSize,
                search,
                UserStatus.Active),
            cancellationToken);

        var inactiveResult = await _mediator.Send(
            new GetUsersQuery(
                inactivePageNumber,
                pageSize,
                search,
                UserStatus.Inactive),
            cancellationToken);

        var lockedResult = await _mediator.Send(
            new GetUsersQuery(
                lockedPageNumber,
                pageSize,
                search,
                UserStatus.Locked),
            cancellationToken);

        if (!activeResult.Succeeded)
        {
            TempData["Error"] = activeResult.Error;
        }

        if (!inactiveResult.Succeeded)
        {
            TempData["Error"] = inactiveResult.Error;
        }

        if (!lockedResult.Succeeded)
        {
            TempData["Error"] = lockedResult.Error;
        }

        var activeUsers =
            activeResult.Value
            ?? new PagedResult<UserListItem>(
                Array.Empty<UserListItem>(),
                activePageNumber,
                pageSize,
                0);

        var inactiveUsers =
            inactiveResult.Value
            ?? new PagedResult<UserListItem>(
                Array.Empty<UserListItem>(),
                inactivePageNumber,
                pageSize,
                0);

        var lockedUsers =
            lockedResult.Value
            ?? new PagedResult<UserListItem>(
                Array.Empty<UserListItem>(),
                lockedPageNumber,
                pageSize,
                0);

        var model = new UsersIndexViewModel
        {
            ActiveUsers = activeUsers,
            InactiveUsers = inactiveUsers,
            LockedUsers = lockedUsers,

            Search = search,
            PageSize = pageSize,
            ActiveTab = activeTab,

            ActivePagination = new PaginationViewModel
            {
                PageNumber = activeUsers.PageNumber,
                PageSize = pageSize,
                TotalCount = activeUsers.TotalCount,
                TotalPages = activeUsers.TotalPages,
                Action = "Index",
                PageParameterName = "activePageNumber",
                ActiveTab = "active",
                Search = search,

                AdditionalParameters = new Dictionary<string, string?>
                {
                    ["inactivePageNumber"] =
                        inactiveUsers.PageNumber.ToString(),

                    ["lockedPageNumber"] =
                        lockedUsers.PageNumber.ToString()
                }
            },

            InactivePagination = new PaginationViewModel
            {
                PageNumber = inactiveUsers.PageNumber,
                PageSize = pageSize,
                TotalCount = inactiveUsers.TotalCount,
                TotalPages = inactiveUsers.TotalPages,
                Action = "Index",
                PageParameterName = "inactivePageNumber",
                ActiveTab = "deactive",
                Search = search,

                AdditionalParameters = new Dictionary<string, string?>
                {
                    ["activePageNumber"] =
                        activeUsers.PageNumber.ToString(),

                    ["lockedPageNumber"] =
                        lockedUsers.PageNumber.ToString()
                }
            },

            LockedPagination = new PaginationViewModel
            {
                PageNumber = lockedUsers.PageNumber,
                PageSize = pageSize,
                TotalCount = lockedUsers.TotalCount,
                TotalPages = lockedUsers.TotalPages,
                Action = "Index",
                PageParameterName = "lockedPageNumber",
                ActiveTab = "locked",
                Search = search,

                AdditionalParameters = new Dictionary<string, string?>
                {
                    ["activePageNumber"] =
                        activeUsers.PageNumber.ToString(),

                    ["inactivePageNumber"] =
                        inactiveUsers.PageNumber.ToString()
                }
            }
        };

        return View(model);
    }

    [HttpGet]
    [Permission(PermissionConstants.Roles.View)]
    public async Task<IActionResult> Roles(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserRolesQuery(userId), cancellationToken);

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
                RoleId = x.roleDto.RoleId,
                RoleName = x.roleDto.RoleName,
                Selected = x.Selected
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Roles.View)]
    public async Task<IActionResult> Roles(UserRolesViewModel model, CancellationToken cancellationToken)
    {
        var selectedRoleIds = model.Roles
            .Where(x => x.Selected)
            .Select(x => x.RoleId)
            .ToList();

        var result = await _mediator.Send(new AssignRolesCommand(model.UserId, selectedRoleIds), cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
            return View(model);
        }

        TempData["Success"] = "نقش‌های کاربر با موفقیت بروزرسانی شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Permission(PermissionConstants.Users.Edit)]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var profileResult = await _mediator.Send(new GetUserProfileQuery(id), cancellationToken);

        if (!profileResult.Succeeded)
        {
            TempData["Error"] = profileResult.Error;
            return RedirectToAction(nameof(Index));
        }

        var rolesResult = await _mediator.Send(new GetUserRolesQuery(id), cancellationToken);

        if (!rolesResult.Succeeded)
        {
            TempData["Error"] = rolesResult.Error;
            return RedirectToAction(nameof(Index));
        }

        var securityResult = await _mediator.Send(new GetUserSecurityQuery(id), cancellationToken);

        if (!securityResult.Succeeded)
        {
            TempData["Error"] = securityResult.Error;
            return RedirectToAction(nameof(Index));
        }

        var model = new EditUserViewModel
        {
            UserId = id,

            Profile = new UserProfileViewModel
            {
                UserId = profileResult.Value!.UserId,
                UserName = profileResult.Value.UserName,
                FirstName = profileResult.Value.FirstName,
                LastName = profileResult.Value.LastName,
                Email = profileResult.Value.Email,
                PhoneNumber = profileResult.Value.PhoneNumber
            },

            Roles = new UserRolesViewModel
            {
                UserId = rolesResult.Value!.UserId,
                UserName = rolesResult.Value.UserName,
                Roles = rolesResult.Value.Roles
                .Select(x => new UserRoleItemViewModel
                {
                    RoleId = x.roleDto.RoleId,
                    RoleName = x.roleDto.RoleName,
                    Selected = x.Selected
                }).ToList()
            },

            Security = new UserSecurityViewModel
            {
                UserId = securityResult.Value!.UserId,
                UserName = securityResult.Value.UserName,
                IsLocked = securityResult.Value.IsLocked,
                IsActive = securityResult.Value.IsActive
            }
        };

        return View(model);
    }

    [HttpGet]
    [Permission(PermissionConstants.Users.Profile)]
    public async Task<IActionResult> UserProfile(CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is not Guid currentUserId)
            return RedirectToAction("index", "Home");

        var profileResult = await _mediator.Send(new GetUserProfileQuery(currentUserId), cancellationToken);
        var model = new UserProfileViewModel
        {
            UserId = currentUserId,
            FirstName = profileResult.Value?.FirstName ?? "",
            LastName = profileResult.Value?.LastName ?? "",
            Email = profileResult.Value?.Email ?? "",
            PhoneNumber = profileResult?.Value?.PhoneNumber ?? "",
            UserName = profileResult?.Value?.UserName ?? "",
            DisplayName = string.Concat(profileResult?.Value?.FirstName ?? "" , profileResult?.Value?.LastName ?? ""),
            Roles = _currentUser.Roles
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Users.Profile)]
    public async Task<IActionResult> UpdateProfile(UserProfileViewModel model, CancellationToken cancellationToken)
    {
        if (model?.UserId is not Guid userId)
        {
            TempData["Error"] = "کاربری با مشخصات داده شده یافت نشد";
            return RedirectToAction(nameof(Edit), new { id = model.UserId });
        }

        var result = await _mediator.Send(new UpdateUserProfileCommand(userId, model.FirstName, model.LastName, model.Email, model.PhoneNumber), cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
            return RedirectToAction(nameof(Edit), new { id = model.UserId });
        }


        TempData["Success"] = "اطلاعات کاربر با موفقیت بروزرسانی شد.";

        return RedirectToAction(nameof(Edit), new { id = model.UserId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Roles.Edit)]
    public async Task<IActionResult> UpdateRoles(UserRolesViewModel model, CancellationToken cancellationToken)
    {
        var selectedRoleIds = model.Roles
            .Where(x => x.Selected)
            .Select(x => x.RoleId)
            .ToList();

        var result = await _mediator.Send(new AssignRolesCommand(model.UserId, selectedRoleIds), cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
            return RedirectToAction(nameof(Edit), new { id = model.UserId });
        }

        TempData["Success"] = "نقش‌های کاربر با موفقیت بروزرسانی شد.";

        return RedirectToAction(nameof(Edit), new { id = model.UserId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Users.Edit)]
    public async Task<IActionResult> UpdateStatus(Guid userId, bool isActive, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateUserStatusCommand(userId, isActive), cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = result.Error;
        }
        else
        {
            TempData["Success"] = "وضعیت کاربر تغییر کرد.";
        }

        return RedirectToAction(nameof(Edit), new { id = userId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Users.Edit)]
    public async Task<IActionResult> Lock(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LockUserCommand(userId), cancellationToken);

        TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded ? "کاربر قفل شد." : result.Error;

        return RedirectToAction(nameof(Edit), new { id = userId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Users.Edit)]
    public async Task<IActionResult> Unlock(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UnlockUserCommand(userId), cancellationToken);
        TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded ? "قفل کاربر باز شد." : result.Error;
        return RedirectToAction(nameof(Edit), new { id = userId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permission(PermissionConstants.Users.Edit)]
    public async Task<IActionResult> ResetPassword(Guid userId, string newPassword, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ResetPasswordCommand(userId, newPassword), cancellationToken);

        TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded ? "رمز عبور تغییر کرد." : result.Error;

        return RedirectToAction(nameof(Edit), new { id = userId });
    }

}