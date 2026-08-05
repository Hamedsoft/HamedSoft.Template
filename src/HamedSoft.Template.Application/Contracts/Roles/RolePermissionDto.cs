namespace HamedSoft.Template.Application.Contracts.Roles;

public sealed record RolePermissionDto(
    Guid PermissionId,
    string PermissionName,
    bool IsAssigned);