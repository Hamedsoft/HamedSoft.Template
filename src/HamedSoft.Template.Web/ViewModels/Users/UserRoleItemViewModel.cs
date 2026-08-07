namespace HamedSoft.Template.Web.ViewModels.Users;

public sealed class UserRoleItemViewModel
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public bool Selected { get; set; }
}