namespace HamedSoft.Template.Web.ViewModels.Users;

public sealed class UserRolesViewModel
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public List<UserRoleItemViewModel> Roles { get; set; } = [];
}