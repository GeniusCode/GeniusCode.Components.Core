using System.Collections.Generic;

namespace GeniusCode.Components.Caching.Implementations
{
    public abstract class CacheAdapterBase<TKey,T> : ICacheAdapter<TKey,T>
    {

        #region constructors

        protected CacheAdapterBase(IEnumerable<TKey> whiteList, IEnumerable<TKey> blackList) : this()
        {
            if (whiteList != null) Whitelist.AddRange(whiteList);
            if (blackList != null) Blacklist.AddRange(blackList);
        }

        protected CacheAdapterBase() 
        {
            Whitelist = new List<TKey>();
            Blacklist = new List<TKey>();

            CacheAllDefault = true;
        }
        #endregion


        #region public members

        public bool CacheAllDefault { get; internal set; }

        public void CacheObject(TKey cacheKey, T toCache)
        {
            if (!CalculateShouldItemBeCached(cacheKey, toCache)) return;

            OnBeforeCacheItem(cacheKey,toCache);
            Perform_CacheObject(cacheKey, toCache);
            OnCacheItem(cacheKey, toCache);
        }

        public void RemoveCachedItem(TKey cacheKey)
        {
            OnBeforeRemoveCachedItem(cacheKey);
            Perform_RemoveCachedItem(cacheKey);
            OnRemoveCachedItem(cacheKey);
        }
        public List<TKey> Blacklist { get; private set; }

        public bool CacheContainsItem(TKey cacheKey)
        {
            return Perform_CheckCacheContainsItem(cacheKey);
        }
        public T GetCachedItem(TKey cacheKey)
        {
            return Perform_GetCachedObject(cacheKey);
        }

        public List<TKey> Whitelist { get; private set; }
        #endregion

        #region Protected Members
        
        protected virtual void OnBeforeRemoveCachedItem(TKey cacheKey) {}
        protected virtual void OnRemoveCachedItem(TKey cacheKey) { }
        protected virtual void OnBeforeCacheItem(TKey cacheKey, T toCache) { }
        protected virtual void OnCacheItem(TKey cacheKey, T toCache) { }

        protected virtual bool CalculateShouldItemBeCached(TKey cacheKey, T toCache)
        {
            if (Blacklist.Contains(cacheKey))
                return false;

            return CacheAllDefault || Whitelist.Contains(cacheKey);
        }

        #endregion

        #region abstract
        protected abstract T Perform_GetCachedObject(TKey cacheKey);
        protected abstract void Perform_CacheObject(TKey cacheKey, T toCache);
        protected abstract void Perform_RemoveCachedItem(TKey cacheKey);
        protected abstract void Perform_ClearCache();
        protected abstract bool Perform_CheckCacheContainsItem(TKey cacheKey);
        protected abstract bool Perform_TryGetItem(TKey cachekey, out T result);
        #endregion



        #region Interface implementation

        void ICacheAdapter<TKey,T>.CacheObject(TKey cacheKey, T toCache)
        {
            CacheObject(cacheKey, toCache);
        }

        T ICacheAdapter<TKey,T>.GetCachedItem(TKey cacheKey)
        {
            return GetCachedItem(cacheKey);
        }

        bool ICacheAdapter<TKey, T>.TryGetItem(TKey cacheKey, out T result)
        {
            return Perform_TryGetItem(cacheKey, out result);
        }

        #endregion

        protected bool DoubleCallTryGet(TKey cachekey, out T result)
        {
                        result = default(T);
            if (Perform_CheckCacheContainsItem(cachekey))
            {
                result = Perform_GetCachedObject(cachekey);
                return true;
            }
            return false;
        }



        
    }
}
