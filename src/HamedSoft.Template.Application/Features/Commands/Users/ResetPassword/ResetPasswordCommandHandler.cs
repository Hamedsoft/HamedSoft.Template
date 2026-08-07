using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.ResetPassword;

public sealed class ResetPasswordCommandHandler
    : ICommandHandler<ResetPasswordCommand, Result>
{
    private readonly IUserManagementService _userManagementService;


    public ResetPasswordCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }


    public async Task<Result> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.ResetPasswordAsync(
            request.UserId,
            request.NewPassword,
            cancellationToken);
    }
}