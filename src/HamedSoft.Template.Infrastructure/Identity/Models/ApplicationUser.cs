using HamedSoft.Template.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace HamedSoft.Template.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public UserProfile Profile { get; set; } = default!;
}