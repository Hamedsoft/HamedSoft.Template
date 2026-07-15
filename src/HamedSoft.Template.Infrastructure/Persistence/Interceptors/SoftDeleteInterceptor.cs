using HamedSoft.Template.Application.Abstractions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using HamedSoft.Template.Application.Contracts.Common;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Infrastructure.Persistence.Interceptors;

public sealed class SoftDeleteInterceptor
    : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SoftDeleteInterceptor(ICurrentUser currentUser, IDateTimeProvider dateTimeProvider)
    {
        _currentUser = currentUser;
        _dateTimeProvider = dateTimeProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplySoftDelete(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    private void ApplySoftDelete(DbContext? context)
    {
        if (context == null)
            return;

        var deletedEntries = context.ChangeTracker.Entries<AuditableEntity<Guid>>().Where(x => x.State == EntityState.Deleted);
        foreach (var entry in deletedEntries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.SetDeleted(_dateTimeProvider.UtcNow, _currentUser.UserId);
        }
    }
}