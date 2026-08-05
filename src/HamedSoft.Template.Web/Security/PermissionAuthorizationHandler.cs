using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Web.Security;
using Microsoft.AspNetCore.Authorization;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var hasAllPermission = context.User.HasClaim(
            CustomClaimTypes.Permission,
            SystemPermissions.All);


        var hasRequiredPermission = context.User.HasClaim(
            CustomClaimTypes.Permission,
            requirement.Permission);


        if (hasAllPermission || hasRequiredPermission)
        {
            context.Succeed(requirement);
        }


        return Task.CompletedTask;
    }
}