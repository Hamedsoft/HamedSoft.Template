using HamedSoft.Template.Application.Contracts.Settings;

namespace HamedSoft.Template.Application.Contracts.Repositories.Reads;

/// <summary>
/// Provides read-only access to application settings.
/// </summary>
public interface ISettingReadRepository
{
    Task<SettingDto?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<SettingDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<SettingDto>> GetByContextAsync(string module, string feature, string category, CancellationToken cancellationToken = default);
}