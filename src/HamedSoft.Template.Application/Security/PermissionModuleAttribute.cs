namespace HamedSoft.Template.Application.Security;

[AttributeUsage(
    AttributeTargets.Class,
    AllowMultiple = false,
    Inherited = false)]
public sealed class PermissionModuleAttribute : Attribute
{
    public string Name { get; }

    public PermissionModuleAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Permission module name cannot be empty.",
                nameof(name));

        Name = name;
    }
}