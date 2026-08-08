using HamedSoft.Template.Application.Security;
using Microsoft.AspNetCore.Authorization;

namespace HamedSoft.Template.Web.Security;

public sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionChecker _permissionChecker;

    public PermissionAuthorizationHandler(
        IPermissionChecker permissionChecker)
    {
        _permissionChecker = permissionChecker;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var hasAllPermission = context.User.HasClaim(
            CustomClaimTypes.Permission,
            SystemPermissions.All);

        if (hasAllPermission)
        {
            context.Succeed(requirement);
            return;
        }

        var hasPermission =
            await _permissionChecker.HasPermissionAsync(
                requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}