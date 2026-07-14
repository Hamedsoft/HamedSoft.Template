using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Contracts.Common;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Features.Commands.Auth.ChangePassword;

public sealed class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, Result>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICurrentUser _currentUser;

    public ChangePasswordHandler(IAuthenticationService authenticationService, ICurrentUser currentUser)
    {
        _authenticationService = authenticationService;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is null)
            return Result.Failure("ابتدا می بایست وارد نرم افزار شوید.");

        return await _authenticationService.ChangePasswordAsync(UserId.Create(_currentUser.UserId.Value), request.CurrentPassword, request.NewPassword, cancellationToken);
    }
}