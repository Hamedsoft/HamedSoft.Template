using System.Reflection;
using HamedSoft.Template.Application.Contracts.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace HamedSoft.Template.Infrastructure.Identity.Permissions;

public sealed class PermissionDiscoveryService
    : IPermissionDiscoveryService
{
    private readonly Assembly _assembly;

    private const string PermissionPrefix = "Permission:";


    public PermissionDiscoveryService()
    {
        _assembly = Assembly.GetEntryAssembly()!;
    }


    public IReadOnlyCollection<string> Discover()
    {
        var permissions = _assembly
            .GetTypes()
            .SelectMany(type => type.GetMethods())
            .SelectMany(method =>
                method.GetCustomAttributes<AuthorizeAttribute>())
            .Where(attribute =>
                attribute.Policy is not null &&
                attribute.Policy.StartsWith(PermissionPrefix))
            .Select(attribute =>
                attribute.Policy![PermissionPrefix.Length..])
            .Distinct()
            .ToList();


        return permissions;
    }
}