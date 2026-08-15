using HamedSoft.Template.Application.Contracts.Roles;

namespace HamedSoft.Template.Application.Contracts.Users;

public sealed record UserRolesDto(
    Guid UserId,
    string UserName,
    IReadOnlyCollection<SelectRole> Roles);