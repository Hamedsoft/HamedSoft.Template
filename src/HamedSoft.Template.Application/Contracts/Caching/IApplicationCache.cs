namespace HamedSoft.Template.Application.Contracts.Caching;

/// <summary>
/// Provides application-level caching operations.
/// </summary>
public interface IApplicationCache
{
    bool TryGetValue<T>(string key, out T? value);

    T? Get<T>(string key);

    void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null);

    void Remove(string key);
}