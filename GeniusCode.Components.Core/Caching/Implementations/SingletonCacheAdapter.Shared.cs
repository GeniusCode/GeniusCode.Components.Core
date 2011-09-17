using System;
using System.Collections.Generic;
using System.Linq;

namespace GeniusCode.Components.Caching.Implementations
{
    public class SingletonCacheAdapter<TKey,T> : CacheAdapterBase<TKey,T>
    {

        //TODO: Inspect if this is a good practice or not...
        private readonly static Dictionary<TKey, T> staticCache = new Dictionary<TKey, T>();

        protected virtual Dictionary<TKey, T> CacheInstance
        {
            get
            {
                return staticCache;
            }
        }



        protected override T Perform_GetCachedObject(TKey cacheKey)
        {
            return CacheInstance[cacheKey];
        }

        protected override void Perform_CacheObject(TKey cacheKey, T toCache)
        {
            // update cache if it already exists
            if (this.CacheContainsItem(cacheKey))
                CacheInstance[cacheKey] = toCache;
            else // add cache if it doesn't exist
                CacheInstance.Add(cacheKey, toCache);
        }

        protected override void Perform_RemoveCachedItem(TKey cacheKey)
        {
            CacheInstance.Remove(cacheKey);
        }

        protected override void Perform_ClearCache()
        {
            var q = from i in CacheInstance
                    select i;

            Array.ForEach(q.ToArray(), r => Perform_RemoveCachedItem(r.Key));
        }

        protected override bool Perform_CheckCacheContainsItem(TKey cacheKey)
        {
            return CacheInstance.ContainsKey(cacheKey);
        }


        protected override bool Perform_TryGetItem(TKey cachekey, out T result)
        {
            return CacheInstance.TryGetValue(cachekey, out result);
        }
    }
}
