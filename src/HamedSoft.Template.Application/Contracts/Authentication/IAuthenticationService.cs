using HamedSoft.Template.Application.Features.Commands.Auth.Login;
using HamedSoft.Template.Application.Features.Commands.Auth.Register;
using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Contracts.Authentication;

public interface IAuthenticationService
{
    Task<Result<HamedSoft.Template.Application.Features.Commands.Auth.Login.LoginResult>> LoginAsync(LoginCommand request, CancellationToken cancellationToken = default);

    Task<Result<HamedSoft.Template.Application.Features.Commands.Auth.Register.RegisterResult>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
}