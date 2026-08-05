namespace HamedSoft.Template.Web.ViewModels.Roles;

public sealed class RolePermissionViewModel
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public List<PermissionItemViewModel> Permissions { get; set; } = [];

    public List<Guid> PermissionIds { get; set; } = [];
}


public sealed class PermissionItemViewModel
{
    public Guid PermissionId { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsAssigned { get; set; }
}