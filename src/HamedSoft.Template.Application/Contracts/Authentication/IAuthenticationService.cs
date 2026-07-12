using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Contracts.Authentication;

public interface IAuthenticationService
{
    Task<Result<LoginResult>> LoginAsync(string identifier, string password, CancellationToken cancellationToken = default);

    Task<Result<RegisterResult>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
}