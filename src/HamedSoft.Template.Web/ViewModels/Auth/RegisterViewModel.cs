using System.ComponentModel.DataAnnotations;

namespace HamedSoft.Template.Web.ViewModels.Auth;

public class RegisterViewModel
{
    [Required(ErrorMessage ="{0} را وارد کنید")]
    [Display(Name = "نام کاربری")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} را وارد کنید")]
    [DataType(DataType.Password)]
    [Display(Name = "کلمه عبور")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} را وارد کنید")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Display(Name = "تکرار کلمه عبور")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Display(Name = "نام")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Display(Name = "نام خانوادگی")]
    public string LastName { get; set; } = string.Empty;
}