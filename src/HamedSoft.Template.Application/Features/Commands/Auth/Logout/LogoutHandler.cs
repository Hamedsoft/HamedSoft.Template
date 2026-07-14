using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Messaging;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Logout;

public sealed class LogoutHandler : ICommand<LogoutCommand>
{
    private readonly IAuthenticationService _authenticationService;

    public LogoutHandler(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.LogoutAsync(cancellationToken);
    }
}