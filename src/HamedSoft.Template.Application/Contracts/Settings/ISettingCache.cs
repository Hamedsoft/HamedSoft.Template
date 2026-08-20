namespace HamedSoft.Template.Application.Contracts.Settings;

/// <summary>
/// Provides caching capabilities for application settings.
/// </summary>
public interface ISettingCache
{
    Task<SettingDto?> GetAsync(string key, CancellationToken cancellationToken = default);

    Task SetAsync(SettingDto setting, CancellationToken cancellationToken = default);

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveAllAsync(CancellationToken cancellationToken = default);
}