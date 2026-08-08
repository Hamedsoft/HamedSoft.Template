namespace HamedSoft.Template.Application.Contracts.Roles;

public sealed record RolePermissionsDto(
    Guid RoleId,
    string RoleName,
    IReadOnlyList<RolePermissionItemDto> Permissions);