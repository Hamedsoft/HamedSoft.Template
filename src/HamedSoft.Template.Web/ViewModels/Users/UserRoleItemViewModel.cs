using System.ComponentModel.DataAnnotations;

namespace HamedSoft.Template.Web.ViewModels.Users;

public sealed class UserRoleItemViewModel
{
    [Display(Name = "شناسه نقش")]
    public Guid RoleId { get; set; }

    [Display(Name = "نام نقش")]
    public string RoleName { get; set; } = string.Empty;

    public bool Selected { get; set; }
}