
namespace GeniusCode.Components.Caching
{
    public interface ICacheAdapter<TKey, T>
    {
        System.Collections.Generic.List<TKey> Blacklist { get; }
        bool CacheContainsItem(TKey cacheKey);
        void CacheObject(TKey cacheKey, T toCache);
        T GetCachedItem(TKey cacheKey);
        bool CacheAllDefault { get; }
        void RemoveCachedItem(TKey cacheKey);
        System.Collections.Generic.List<TKey> Whitelist { get; }
        bool TryGetItem(TKey cacheKey, out T result);
    }
}
