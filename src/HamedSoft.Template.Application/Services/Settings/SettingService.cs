using System.Globalization;
using System.Text.Json;
using HamedSoft.Template.Application.Contracts.Caching;
using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.Settings;
using HamedSoft.Template.Application.Contracts.UnitOfWork;

namespace HamedSoft.Template.Application.Settings;

/// <summary>
/// Provides application settings access with application-level caching.
/// </summary>
internal sealed class SettingService : ISettingService
{
    private const string CacheKeyPrefix = "setting:";

    private readonly ISettingReadRepository _readRepository;
    private readonly ISettingWriteRepository _writeRepository;
    private readonly IApplicationUnitOfWork _unitOfWork;
    private readonly IApplicationCache _cache;

    public SettingService(
        ISettingReadRepository readRepository,
        ISettingWriteRepository writeRepository,
        IApplicationUnitOfWork unitOfWork,
        IApplicationCache cache)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<SettingDto?> GetAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var cacheKey = BuildCacheKey(key);

        if (_cache.TryGetValue<SettingDto>(cacheKey, out var cached))
            return cached;

        var setting = await _readRepository.GetByKeyAsync(
            key,
            cancellationToken);

        if (setting is not null)
            _cache.Set(cacheKey, setting);

        return setting;
    }

    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        var setting = await GetAsync(key, cancellationToken);

        if (setting is null)
            return default;

        return ConvertValue<T>(setting);
    }

    public async Task SetAsync(
    string key,
    string value,
    CancellationToken cancellationToken = default)
    {
        var setting = await _writeRepository.GetByKeyAsync(
            key,
            cancellationToken);

        if (setting is null)
            throw new InvalidOperationException(
                $"Setting '{key}' was not found.");

        setting.ChangeValue(value);

        await _writeRepository.UpdateAsync(
            setting,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        _cache.Remove(BuildCacheKey(key));
        _cache.Remove("settings:all");
    }

    public async Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var entity = await _writeRepository.GetByKeyAsync(
            key,
            cancellationToken);

        if (entity is null)
            return;

        await _writeRepository.RemoveAsync(
            entity,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _cache.Remove(BuildCacheKey(key));
        _cache.Remove("settings:all");
    }

    private static string BuildCacheKey(string key)
        => $"{CacheKeyPrefix}{key}";

    private static T? ConvertValue<T>(SettingDto setting)
    {
        var targetType = Nullable.GetUnderlyingType(typeof(T))
                         ?? typeof(T);

        if (targetType == typeof(string))
            return (T)(object)setting.Value;

        if (targetType == typeof(int))
            return (T)(object)int.Parse(
                setting.Value,
                CultureInfo.InvariantCulture);

        if (targetType == typeof(long))
            return (T)(object)long.Parse(
                setting.Value,
                CultureInfo.InvariantCulture);

        if (targetType == typeof(decimal))
            return (T)(object)decimal.Parse(
                setting.Value,
                CultureInfo.InvariantCulture);

        if (targetType == typeof(bool))
            return (T)(object)bool.Parse(setting.Value);

        if (targetType == typeof(DateTime))
            return (T)(object)DateTime.Parse(
                setting.Value,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind);

        if (targetType == typeof(TimeSpan))
            return (T)(object)TimeSpan.Parse(
                setting.Value,
                CultureInfo.InvariantCulture);

        if (targetType == typeof(JsonDocument))
            return (T)(object)JsonDocument.Parse(setting.Value);

        throw new InvalidOperationException(
            $"Setting '{setting.Key}' cannot be converted to {targetType.Name}.");
    }
    public async Task<IReadOnlyCollection<SettingDto>> GetAllAsync(
    CancellationToken cancellationToken = default)
    {
        const string cacheKey = "settings:all";

        if (_cache.TryGetValue<IReadOnlyCollection<SettingDto>>(
            cacheKey,
            out var cached))
        {
            return cached;
        }

        var settings = await _readRepository.GetAllAsync(
            cancellationToken);

        _cache.Set(cacheKey, settings);

        return settings;
    }
}