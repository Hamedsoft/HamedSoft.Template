namespace HamedSoft.Template.Application.Contracts.Permissions;

public sealed record PermissionDefinition(
    string Name,
    string Module,
    string Category,
    string DisplayName,
    string? Description);