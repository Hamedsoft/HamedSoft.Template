namespace HamedSoft.Template.Application.Contracts.Permissions;

public interface IPermissionDiscoveryService
{
    IReadOnlyCollection<PermissionDefinition> Discover();
}