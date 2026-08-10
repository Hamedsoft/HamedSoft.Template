using HamedSoft.Template.Application.Contracts.Security;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Web.Security;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace HamedSoft.Template.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IPermissionChecker _permissionChecker;

    public HomeController(ILogger<HomeController> logger, ICurrentUser currentUser, IPermissionChecker permissionChecker)
    {
        _logger = logger;
        _currentUser = currentUser;
        _permissionChecker = permissionChecker;
    }

    [Permission(PermissionConstants.Common.Home)]
    public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId;
        var authenticated = _currentUser.IsAuthenticated;

        var canViewUsers = await _permissionChecker.HasPermissionAsync("Users.View", cancellationToken);

        var canEditUsers = await _permissionChecker.HasPermissionAsync("Users.Edit", cancellationToken);

        var isAuthenticated = User.Identity?.IsAuthenticated;

        var name = User.Identity?.Name;
        ViewData["name"] = name;
        ViewData["isAuthenticated"] = isAuthenticated;
        return View();
    }
}
