using Microsoft.AspNetCore.Authorization;

namespace HamedSoft.Template.Web.Security;

public sealed class PermissionAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "Permission:";

    public PermissionAttribute(string permission)
    {
        Policy = $"{PolicyPrefix}{permission}";
    }
}