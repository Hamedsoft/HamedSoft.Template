using System.Security.Claims;
using HamedSoft.Template.Application.Security;

namespace HamedSoft.Template.Web.Security;

public static class PermissionClaimsExtensions
{
    public static bool HasPermission(
        this ClaimsPrincipal user,
        string permission)
    {
        if (user.Identity?.IsAuthenticated != true)
            return false;

        if (user.HasClaim(
                CustomClaimTypes.Permission,
                SystemPermissions.All))
        {
            return true;
        }

        return user.HasClaim(
            CustomClaimTypes.Permission,
            permission);
    }
}