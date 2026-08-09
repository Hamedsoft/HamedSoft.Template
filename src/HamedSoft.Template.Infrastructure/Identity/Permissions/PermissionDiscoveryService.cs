using System.Reflection;
using HamedSoft.Template.Application.Contracts.Permissions;
using HamedSoft.Template.Application.Security;

namespace HamedSoft.Template.Infrastructure.Identity.Permissions;

public sealed class PermissionDiscoveryService
    : IPermissionDiscoveryService
{
    private readonly Assembly _assembly;

    public PermissionDiscoveryService()
    {
        _assembly = typeof(PermissionConstants).Assembly;
    }

    public IReadOnlyCollection<PermissionDefinition> Discover()
    {
        var definitions = new List<PermissionDefinition>();

        var fields = _assembly
            .GetTypes()
            .SelectMany(type =>
                type.GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.DeclaredOnly))
            .Where(field =>
                field.IsLiteral &&
                !field.IsInitOnly &&
                field.FieldType == typeof(string));

        foreach (var field in fields)
        {
            var definition =
                field.GetCustomAttribute<PermissionDefinitionAttribute>();

            if (definition is null)
                continue;

            var module =
                field.DeclaringType?
                    .GetCustomAttribute<PermissionModuleAttribute>();

            var category =
                field.DeclaringType?
                    .GetCustomAttribute<PermissionCategoryAttribute>();

            if (module is null)
            {
                throw new InvalidOperationException(
                    $"Permission '{field.Name}' in " +
                    $"'{field.DeclaringType?.FullName}' " +
                    "does not have a PermissionModuleAttribute.");
            }

            if (category is null)
            {
                throw new InvalidOperationException(
                    $"Permission '{field.Name}' in " +
                    $"'{field.DeclaringType?.FullName}' " +
                    "does not have a PermissionCategoryAttribute.");
            }

            var permissionName =
                field.GetRawConstantValue() as string;

            if (string.IsNullOrWhiteSpace(permissionName))
            {
                throw new InvalidOperationException(
                    $"Permission '{field.DeclaringType?.FullName}.{field.Name}' " +
                    "has an empty permission name.");
            }

            definitions.Add(
                new PermissionDefinition(
                    permissionName,
                    module.Name,
                    category.Name,
                    definition.DisplayName,
                    definition.Description));
        }

        return definitions
            .DistinctBy(x => x.Name)
            .ToList();
    }
}