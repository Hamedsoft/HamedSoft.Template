using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Infrastructure.Identity.Models;

public class Permission : Entity<Guid>
{
    public string Name { get; private set; } = default!;

    public string Module { get; private set; } = default!;

    public string Category { get; private set; } = default!;

    public string DisplayName { get; private set; } = default!;

    public string? Description { get; private set; }

    private Permission()
    {
    }

    public Permission(
        Guid id,
        string name,
        string module,
        string category,
        string displayName,
        string? description = null)
        : base(id)
    {
        Name = name;
        Module = module;
        Category = category;
        DisplayName = displayName;
        Description = description;
    }

    public ICollection<RolePermission> Roles { get; private set; }
        = new List<RolePermission>();

    public void UpdateMetadata(
        string module,
        string category,
        string displayName,
        string? description)
    {
        Module = module;
        Category = category;
        DisplayName = displayName;
        Description = description;
    }
}