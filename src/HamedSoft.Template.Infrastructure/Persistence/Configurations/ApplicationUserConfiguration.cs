using HamedSoft.Template.Domain.UserProfiles;
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
            .HasForeignKey<ApplicationUser>(x => x.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}