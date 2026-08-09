namespace HamedSoft.Template.Application.Security;

[AttributeUsage(
    AttributeTargets.Field,
    AllowMultiple = false,
    Inherited = false)]
public sealed class PermissionDefinitionAttribute : Attribute
{
    public string DisplayName { get; }

    public string? Description { get; }

    public PermissionDefinitionAttribute(
        string displayName,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException(
                "Permission display name cannot be empty.",
                nameof(displayName));

        DisplayName = displayName;
        Description = description;
    }
}