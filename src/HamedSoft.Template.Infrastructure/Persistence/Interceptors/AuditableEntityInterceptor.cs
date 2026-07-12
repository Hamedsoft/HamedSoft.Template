using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Abstractions.Common;
using HamedSoft.Template.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HamedSoft.Template.Infrastructure.Persistence.Interceptors;

public sealed class AuditableEntityInterceptor
    : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuditableEntityInterceptor(ICurrentUser currentUser, IDateTimeProvider dateTimeProvider)
    {
        _currentUser = currentUser;
        _dateTimeProvider = dateTimeProvider;
    }


    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }


    private void ApplyAudit(DbContext? context)
    {
        if (context == null)
            return;

        var entries = context.ChangeTracker.Entries<AuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.SetCreated(_dateTimeProvider.UtcNow, _currentUser.UserId);

            if (entry.State == EntityState.Modified)
                entry.Entity.SetModified(_dateTimeProvider.UtcNow, _currentUser.UserId);
        }
    }
}