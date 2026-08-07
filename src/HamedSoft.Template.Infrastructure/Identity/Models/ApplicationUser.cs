using Microsoft.AspNetCore.Identity;

namespace HamedSoft.Template.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public bool IsActive { get; set; } = true;
}