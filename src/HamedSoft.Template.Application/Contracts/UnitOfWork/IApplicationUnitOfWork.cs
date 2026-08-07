namespace HamedSoft.Template.Application.Contracts.UnitOfWork;

public interface IApplicationUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);

}
