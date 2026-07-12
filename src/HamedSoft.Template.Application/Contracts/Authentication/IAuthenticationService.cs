using HamedSoft.Template.SharedKernel.Common;
namespace HamedSoft.Template.Application.Contracts.Authentication;

public interface IAuthenticationService
{
    Task<Result<LoginResult>> LoginAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default);
}
