namespace HamedSoft.Template.Infrastructure.Initialization;

public sealed class InfrastructureInitializer
{
    private readonly IEnumerable<IInitializer> _initializers;

    public InfrastructureInitializer(
        IEnumerable<IInitializer> initializers)
    {
        _initializers = initializers;
    }

    public async Task InitializeAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var initializer in _initializers)
        {
            await initializer.InitializeAsync(
                cancellationToken);
        }
    }
}