using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.UnlockUser;

public sealed class UnlockUserCommandHandler
    : ICommandHandler<UnlockUserCommand, Result>
{
    private readonly IUserManagementService _userManagementService;


    public UnlockUserCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }


    public async Task<Result> Handle(
        UnlockUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.UnlockAsync(
            request.UserId,
            cancellationToken);
    }
}