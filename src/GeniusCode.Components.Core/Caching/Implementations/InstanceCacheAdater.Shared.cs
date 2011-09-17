using System.Collections.Generic;

namespace GeniusCode.Components.Caching.Implementations
{
    public class InstanceCacheAdapter<TKey,T> : SingletonCacheAdapter<TKey,T>
    {
        public InstanceCacheAdapter()
        {
            Cache = new Dictionary<TKey, T>();
        }

        public InstanceCacheAdapter(Dictionary<TKey,T> dictionaryToUse)
        {
            Cache = dictionaryToUse;
        }

        protected Dictionary<TKey, T> Cache;
        protected sealed override Dictionary<TKey, T> CacheInstance
        {
            get
            {
                return Cache; ;
            }
        }

    }
}
