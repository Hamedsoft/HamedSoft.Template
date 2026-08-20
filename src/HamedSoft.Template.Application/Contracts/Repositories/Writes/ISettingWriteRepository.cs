using HamedSoft.Template.Domain.Settings;

namespace HamedSoft.Template.Application.Contracts.Repositories.Writes;

/// <summary>
/// Provides write access to application settings.
/// </summary>
public interface ISettingWriteRepository
{
    Task AddAsync(Setting setting, CancellationToken cancellationToken = default);

    Task UpdateAsync(Setting setting, CancellationToken cancellationToken = default);

    Task RemoveAsync(Setting setting, CancellationToken cancellationToken = default);

    Task<bool> SetValueAsync(string key, string value, CancellationToken cancellationToken = default);

    Task<bool> RemoveByKeyAsync(string key, CancellationToken cancellationToken = default);

    //Todo: migrate from WriteRepository
    Task<Setting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
}