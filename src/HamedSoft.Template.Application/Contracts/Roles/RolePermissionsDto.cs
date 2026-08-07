using HamedSoft.Template.Application.Common.Models;

namespace HamedSoft.Template.Application.Contracts.Roles;

public sealed record RolePermissionsDto(
    Guid RoleId,
    string RoleName,
    IReadOnlyList<LookupItemDto> Permissions);