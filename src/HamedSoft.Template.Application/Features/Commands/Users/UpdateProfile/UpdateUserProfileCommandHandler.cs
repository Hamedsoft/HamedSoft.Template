using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.UpdateProfile;

public sealed class UpdateUserProfileCommandHandler
    : ICommandHandler<UpdateUserProfileCommand, Result>
{
    private readonly IUserManagementService _userManagementService;

    public UpdateUserProfileCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }


    public async Task<Result> Handle(
        UpdateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        var dto = new UserProfileDto(
            request.UserId,
            string.Empty,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber);


        return await _userManagementService.UpdateProfileAsync(
            dto,
            cancellationToken);
    }
}