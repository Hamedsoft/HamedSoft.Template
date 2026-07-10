namespace HamedSoft.Template.Infrastructure.Identity;

public class Permission
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public ICollection<RolePermission> Roles { get; set; } = new List<RolePermission>();
}