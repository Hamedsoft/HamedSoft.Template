using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.LockUser;

public sealed class LockUserCommandHandler
    : ICommandHandler<LockUserCommand, Result>
{
    private readonly IUserManagementService _userManagementService;


    public LockUserCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }


    public async Task<Result> Handle(
        LockUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.LockAsync(
            request.UserId,
            cancellationToken);
    }
}