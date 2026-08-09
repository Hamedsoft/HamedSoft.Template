using System;

namespace HamedSoft.Template.Application.Security;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class PermissionDefinitionAttribute : Attribute
{
    public string Module { get; }
    public string Category { get; }
    public string DisplayName { get; }
    public string? Description { get; }

    public PermissionDefinitionAttribute(string module, string category, string displayName, string? description = null)
    {
        Module = module;
        Category = category;
        DisplayName = displayName;
        Description = description;
    }
}