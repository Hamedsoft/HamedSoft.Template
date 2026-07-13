using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<LoginResult>>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return _authenticationService.LoginAsync(
            request.UserName,
            request.Password,
            cancellationToken);
    }
}