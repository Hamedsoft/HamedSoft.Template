namespace HamedSoft.Template.Application.Contracts.Permissions;

public interface IPermissionDiscoveryService
{
    IReadOnlyCollection<string> Discover();
}