using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Register;

public sealed class RegisterHandler : ICommandHandler<RegisterCommand, Result<RegisterResult>>
{
    private readonly IAuthenticationService _authenticationService;

    public RegisterHandler(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RegisterAsync(request.UserName, request.Password, cancellationToken);

        if (!result.Succeeded)
            return Result<RegisterResult>.Failure(result.Error!);

        var user = result.Value!;

        return Result<RegisterResult>.Success(
            new RegisterResult(user.UserId));
    }
}