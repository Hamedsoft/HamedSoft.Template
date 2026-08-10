using System.ComponentModel.DataAnnotations;

namespace HamedSoft.Template.Web.ViewModels.Roles;

public sealed class EditRoleViewModel
{
    public Guid RoleId { get; set; }

    [Display(Name ="نام نقش")]
    [Required(ErrorMessage = "{0} را وارد کنید.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} باید بین 2 تا 100 کاراکتر باشد.")]
    public string RoleName { get; set; } = string.Empty;
}