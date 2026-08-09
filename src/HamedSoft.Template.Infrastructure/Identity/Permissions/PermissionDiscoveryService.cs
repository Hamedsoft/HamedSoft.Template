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
        return _assembly
            .GetTypes()
            .SelectMany(type =>
                type.GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static))
            .Where(field =>
                field.IsLiteral &&
                !field.IsInitOnly &&
                field.FieldType == typeof(string))
            .Select(field =>
            {
                var value =
                    field.GetRawConstantValue() as string;

                var attribute =
                    field.GetCustomAttribute<
                        PermissionDefinitionAttribute>();

                return new
                {
                    Name = value,
                    Attribute = attribute
                };
            })
            .Where(x =>
                !string.IsNullOrWhiteSpace(x.Name) &&
                x.Attribute is not null)
            .Select(x =>
                new PermissionDefinition(
                    x.Name!,
                    x.Attribute!.Module,
                    x.Attribute.Category,
                    x.Attribute.DisplayName,
                    x.Attribute.Description))
            .DistinctBy(x => x.Name)
            .ToList();
    }
}