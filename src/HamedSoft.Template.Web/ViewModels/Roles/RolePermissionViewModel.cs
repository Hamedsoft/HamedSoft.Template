namespace HamedSoft.Template.Web.ViewModels.Roles;

public sealed class RolePermissionViewModel
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public List<PermissionItemViewModel> Permissions { get; set; } = [];

    public List<Guid> PermissionIds { get; set; } = [];
}
