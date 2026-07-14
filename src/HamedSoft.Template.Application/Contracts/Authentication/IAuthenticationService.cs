using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Contracts.Authentication;

public interface IAuthenticationService
{
    Task<Result<AuthenticatedUser>> LoginAsync(string userName, string password, CancellationToken cancellationToken = default);

    Task<Result<RegisteredUser>> RegisterAsync(string userName, string password, CancellationToken cancellationToken = default);

    Task LogoutAsync(CancellationToken cancellationToken = default);
}