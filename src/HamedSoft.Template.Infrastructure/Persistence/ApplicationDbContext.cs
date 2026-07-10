using System.Linq.Expressions;
using HamedSoft.Template.Domain.Users;
using HamedSoft.Template.Infrastructure.Identity;
using HamedSoft.Template.SharedKernel.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).HasQueryFilter(CreateIsDeletedFilter(entityType.ClrType));
                builder.Entity(entityType.ClrType).Property(nameof(AuditableEntity.RowVersion)).IsRowVersion();
            }
        }
    }
    private static LambdaExpression CreateIsDeletedFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");

        var property = Expression.Property(parameter, nameof(AuditableEntity.IsDeleted));

        var falseConstant = Expression.Constant(false);

        var body = Expression.Equal(property, falseConstant);

        return Expression.Lambda(body, parameter);
    }
}