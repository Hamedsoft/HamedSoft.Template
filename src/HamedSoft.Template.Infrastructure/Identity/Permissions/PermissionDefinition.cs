namespace HamedSoft.Template.Infrastructure.Identity.Permissions;

public sealed record PermissionDefinition(
    string Name,
    string Module,
    string Category,
    string DisplayName,
    string? Description = null);