using HamedSoft.Template.Domain.Users;
using HamedSoft.Template.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HamedSoft.Template.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration
    : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(
        EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(x => x.Profile)
            .WithOne()
            .HasForeignKey<UserProfile>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}