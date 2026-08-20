using HamedSoft.Template.Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace HamedSoft.Template.Infrastructure.Caching;

internal sealed class MemoryApplicationCache : IApplicationCache
{
    private readonly IMemoryCache _cache;

    public MemoryApplicationCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    public bool TryGetValue<T>(
        string key,
        out T? value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void Set<T>(
        string key,
        T value,
        TimeSpan? absoluteExpiration = null)
    {
        if (absoluteExpiration is null)
        {
            _cache.Set(key, value);
            return;
        }

        _cache.Set(
            key,
            value,
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    absoluteExpiration
            });
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}