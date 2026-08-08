namespace HamedSoft.Template.Web.ViewModels.Roles;

public sealed class PermissionItemViewModel
{
    public Guid PermissionId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Module { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsAssigned { get; set; }
}