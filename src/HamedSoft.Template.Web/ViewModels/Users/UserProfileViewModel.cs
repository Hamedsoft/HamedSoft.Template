using System.ComponentModel.DataAnnotations;

namespace HamedSoft.Template.Web.ViewModels.Users;
public sealed class UserProfileViewModel
{
    [Display(Name = "شناسه")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public Guid? UserId { get; set; }

    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name ="نام")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "نام کامل")]
    public string DisplayName { get; init; } = string.Empty;


    [Display(Name = "ایمیل")]
    public string? Email { get; set; }

    [Display(Name = "شماره موبایل")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "نفض")]
    public IReadOnlyCollection<string> Roles { get; init; } = Array.Empty<string>();
}