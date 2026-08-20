namespace HamedSoft.Template.Application.Contracts.Settings;

/// <summary>
/// Provides access to application settings.
/// </summary>
public interface ISettingService
{
    Task<SettingDto?> GetAsync(string key, CancellationToken cancellationToken = default);

    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    Task SetAsync(string key, string value, CancellationToken cancellationToken = default);

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<SettingDto>> GetAllAsync(CancellationToken cancellationToken = default);
}