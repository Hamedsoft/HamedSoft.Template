namespace HamedSoft.Template.Application.Security;

public interface IPermissionChecker
{
    Task<bool> HasPermissionAsync(string permissionName, CancellationToken cancellationToken = default);
}