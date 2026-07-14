using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Login;

public sealed class LoginHandler : ICommandHandler<LoginCommand, Result<LoginResult>>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginHandler(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.LoginAsync(request.UserName, request.Password, cancellationToken);

        if (!result.Succeeded)
            return Result<LoginResult>.Failure(result.Error!);

        var user = result.Value!;

        return Result<LoginResult>.Success(
            new LoginResult(
                user.UserId,
                user.UserName,
                user.DisplayName,
                user.Roles));
    }
}