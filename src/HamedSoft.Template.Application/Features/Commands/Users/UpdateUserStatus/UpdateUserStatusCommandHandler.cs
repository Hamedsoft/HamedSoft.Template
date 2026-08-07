using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.UpdateUserStatus;

public sealed class UpdateUserStatusCommandHandler
    : ICommandHandler<UpdateUserStatusCommand, Result>
{
    private readonly IUserManagementService _userManagementService;


    public UpdateUserStatusCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }


    public async Task<Result> Handle(
        UpdateUserStatusCommand request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.UpdateStatusAsync(
            request.UserId,
            request.IsActive,
            cancellationToken);
    }
}