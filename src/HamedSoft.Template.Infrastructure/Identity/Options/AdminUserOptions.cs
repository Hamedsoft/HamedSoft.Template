namespace HamedSoft.Template.Infrastructure.Identity.Options;

public sealed class AdminUserOptions
{
    public const string SectionName = "Identity:AdminUser";


    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}