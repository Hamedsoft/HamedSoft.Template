using Microsoft.AspNetCore.Identity;

namespace HamedSoft.Template.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
}