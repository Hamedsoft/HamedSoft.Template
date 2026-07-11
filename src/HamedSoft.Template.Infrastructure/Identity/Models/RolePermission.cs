using HamedSoft.Template.SharedKernel.Entities;

namespace HamedSoft.Template.Infrastructure.Identity.Models;

public class RolePermission
{
    public Guid RoleId { get; private set; }

    public ApplicationRole Role { get; private set; } = default!;

    public Guid PermissionId { get; private set; }

    public Permission Permission { get; private set; } = default!;

    private RolePermission()
    {
    }

    public RolePermission(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
}