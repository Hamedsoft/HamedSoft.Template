using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Contracts.Users;

public interface IUserManagementService
{
    Task<Result<IReadOnlyList<UserListItem>>> GetAllAsync(
        CancellationToken cancellationToken = default);


    Task<Result<UserRolesDto>> GetRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result> AssignRolesAsync(
        Guid userId,
        IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken = default);

    Task<Result<UserProfileDto>> GetProfileAsync(
    Guid userId,
    CancellationToken cancellationToken = default);

    Task<Result> UpdateProfileAsync(
        UserProfileDto profile,
        CancellationToken cancellationToken = default);

    //security
    Task<Result<UserSecurityDto>> GetSecurityAsync(
    Guid userId,
    CancellationToken cancellationToken = default);


    Task<Result> ResetPasswordAsync(
        Guid userId,
        string newPassword,
        CancellationToken cancellationToken = default);


    Task<Result> UpdateStatusAsync(
        Guid userId,
        bool isActive,
        CancellationToken cancellationToken = default);


    Task<Result> LockAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    Task<Result> UnlockAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}