using HamedSoft.Template.Application.Common.Models;

namespace HamedSoft.Template.Application.Contracts.Users;

public sealed record UserRolesDto(
    Guid UserId,
    string UserName,
    IReadOnlyCollection<LookupItemDto> Roles);