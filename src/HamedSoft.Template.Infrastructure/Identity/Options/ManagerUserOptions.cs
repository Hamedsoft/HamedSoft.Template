namespace HamedSoft.Template.Infrastructure.Identity.Options;

public sealed class ManagerUserOptions
{
    public const string SectionName = "Identity:ManagerUser";


    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}