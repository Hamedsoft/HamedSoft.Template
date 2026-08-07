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
}