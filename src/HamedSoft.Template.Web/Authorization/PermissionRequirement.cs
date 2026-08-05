using Microsoft.AspNetCore.Authorization;

namespace HamedSoft.Template.Web.Authorization;

public sealed class PermissionRequirement
    : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}