using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.UnitOfWork;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Domain.UserProfiles;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Register;

public sealed class RegisterHandler : ICommandHandler<RegisterCommand, Result<RegisterResult>>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserProfileWriteRepository _userProfileWriteRepository;
    private readonly IApplicationUnitOfWork _unitOfWork;

    public RegisterHandler(IAuthenticationService authenticationService, IUserProfileWriteRepository userProfileWriteRepository, IApplicationUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;
        _userProfileWriteRepository = userProfileWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RegisterAsync(request.UserName, request.Password, cancellationToken);

        if (!result.Succeeded)
            return Result<RegisterResult>.Failure(result.Error!);

        var user = result.Value!;

        var profile = UserProfile.Create(UserProfileId.Create(user.UserId), request.FirstName, request.LastName);

        await _userProfileWriteRepository.AddAsync(profile, cancellationToken);

        await _unitOfWork.SaveChangesAsync();

        return Result<RegisterResult>.Success(
            new RegisterResult(user.UserId));

    }
}