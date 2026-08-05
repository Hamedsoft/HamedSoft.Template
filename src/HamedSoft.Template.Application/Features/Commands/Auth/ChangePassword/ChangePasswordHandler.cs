using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Auth.ChangePassword;

public sealed class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, Result>
{
    private readonly IAuthenticationService _authenticationService;

    public ChangePasswordHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword, cancellationToken);
    }
}