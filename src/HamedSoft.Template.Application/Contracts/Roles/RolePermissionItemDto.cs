namespace HamedSoft.Template.Application.Contracts.Roles;

public sealed record RolePermissionItemDto(
    Guid Id,
    string Name,
    string Module,
    string Category,
    string DisplayName,
    string? Description,
    bool Selected);