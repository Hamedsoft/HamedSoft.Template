using System.ComponentModel.DataAnnotations;
namespace HamedSoft.Template.Web.ViewModels.Auth;
public sealed class ChangePasswordViewModel
{
    [Required(ErrorMessage = "رمز عبور فعلی الزامی است.")]
    [Display(Name = "رمز عبور فعلی")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;


    [Required(ErrorMessage = "رمز عبور جدید الزامی است.")]
    [Display(Name = "رمز عبور جدید")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "رمز عبور حداقل باید ۶ کاراکتر باشد.")]
    public string NewPassword { get; set; } = string.Empty;


    [Required(ErrorMessage = "تکرار رمز عبور الزامی است.")]
    [Display(Name = "تکرار رمز عبور")]
    [Compare(nameof(NewPassword), ErrorMessage = "تکرار رمز عبور صحیح نیست.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}