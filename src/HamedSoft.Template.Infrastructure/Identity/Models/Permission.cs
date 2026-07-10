namespace HamedSoft.Template.Infrastructure.Identity.Models;

public class Permission
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = default!;

    public string? Description { get; private set; }

    private Permission()
    {
    }

    public Permission(string name, string? description = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    public ICollection<RolePermission> Roles { get; private set; } = new List<RolePermission>();
}