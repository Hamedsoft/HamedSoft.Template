namespace HamedSoft.Template.Web.ViewModels.Users;

public sealed class UserSecurityViewModel
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public bool IsLocked { get; set; }

    public bool IsActive { get; set; }
}