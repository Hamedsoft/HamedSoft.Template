using System.Linq.Expressions;
using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Domain.UserProfiles;
using HamedSoft.Template.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(AuditableEntity<Guid>).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).HasQueryFilter(CreateIsDeletedFilter(entityType.ClrType));
                builder.Entity(entityType.ClrType).Property(nameof(AuditableEntity<Guid>.RowVersion)).IsRowVersion();
            }
        }
    }
    private static LambdaExpression CreateIsDeletedFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");

        var property = Expression.Property(parameter, nameof(AuditableEntity<Guid>.IsDeleted));

        var falseConstant = Expression.Constant(false);

        var body = Expression.Equal(property, falseConstant);

        return Expression.Lambda(body, parameter);
    }
}