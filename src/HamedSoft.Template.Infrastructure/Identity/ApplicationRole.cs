using Microsoft.AspNetCore.Identity;

namespace HamedSoft.Template.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public ICollection<RolePermission> Permissions { get; private set; } = new List<RolePermission>();
}