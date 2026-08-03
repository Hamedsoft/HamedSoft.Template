using HamedSoft.Template.Domain.SharedKernel.ValueObjects;
using HamedSoft.Template.Domain.UserProfiles;
using Microsoft.AspNetCore.Identity;

namespace HamedSoft.Template.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser<Guid>
{
}