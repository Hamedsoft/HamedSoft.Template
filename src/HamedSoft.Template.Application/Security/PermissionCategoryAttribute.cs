namespace HamedSoft.Template.Application.Security;

[AttributeUsage(
    AttributeTargets.Class,
    AllowMultiple = false,
    Inherited = false)]
public sealed class PermissionCategoryAttribute : Attribute
{
    public string Name { get; }

    public PermissionCategoryAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Permission category name cannot be empty.",
                nameof(name));

        Name = name;
    }
}