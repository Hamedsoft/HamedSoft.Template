using HamedSoft.Template.Application.Common.Paging;
using HamedSoft.Template.Application.Contracts.Users;

namespace HamedSoft.Template.Application.Contracts.Repositories.Reads;

public interface IUserReadRepository
{
    Task<PagedResult<UserListItem>> GetPagedAsync(
        bool withAdminUser,
        PageRequest pageRequest,
        string? search = null,
        UserStatus? status = null,
        CancellationToken cancellationToken = default);
}