using System.ComponentModel.DataAnnotations;

namespace HamedSoft.Template.Web.ViewModels.Auth;

public sealed class LoginViewModel
{
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Display(Name = "نام کاربری")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} را وارد کنید")]
    [DataType(DataType.Password)]
    [Display(Name = "کلمه عبور")]
    public string Password { get; set; } = string.Empty;

    [Display(Name ="من را به خاطر بسپار")]
    public bool RememberMe { get; set; }
}