using Microsoft.AspNetCore.Authorization;

namespace HamedSoft.Template.Web.Authorization;

public sealed class PermissionAttribute
    : AuthorizeAttribute
{
    public PermissionAttribute(string permission)
    {
        Policy = permission;
    }
}