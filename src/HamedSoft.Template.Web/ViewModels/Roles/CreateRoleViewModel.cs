using System.ComponentModel.DataAnnotations;

namespace HamedSoft.Template.Web.ViewModels.Roles;

public sealed class CreateRoleViewModel
{
    [Required(ErrorMessage = "نام نقش الزامی است.")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "نام نقش باید بین 2 تا 100 کاراکتر باشد.")]
    public string RoleName { get; set; } = string.Empty;
}