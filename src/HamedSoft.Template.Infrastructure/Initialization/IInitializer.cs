namespace HamedSoft.Template.Infrastructure.Initialization;

public interface IInitializer
{
    Task InitializeAsync(
        CancellationToken cancellationToken = default);
}