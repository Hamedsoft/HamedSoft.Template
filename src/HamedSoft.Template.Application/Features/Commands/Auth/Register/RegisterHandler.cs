using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Login;

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
        return _authenticationService.RegisterAsync(
            request.UserName,
            request.Password,
            cancellationToken);
    }
}