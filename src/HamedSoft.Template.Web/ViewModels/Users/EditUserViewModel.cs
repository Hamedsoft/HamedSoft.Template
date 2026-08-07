namespace HamedSoft.Template.Web.ViewModels.Users;

public sealed class EditUserViewModel
{
    public Guid UserId { get; set; }

    public UserProfileViewModel Profile { get; set; } = new();

    public UserRolesViewModel Roles { get; set; } = new();

    public UserSecurityViewModel Security { get; set; } = new();
}