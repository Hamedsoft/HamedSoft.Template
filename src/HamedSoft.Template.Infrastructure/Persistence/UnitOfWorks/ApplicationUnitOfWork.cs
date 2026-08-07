using HamedSoft.Template.Application.Contracts.UnitOfWork;

namespace HamedSoft.Template.Infrastructure.Persistence.UnitOfWorks;

internal sealed class ApplicationUnitOfWork : IApplicationUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public ApplicationUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
       return _context.SaveChangesAsync();
    }
}
