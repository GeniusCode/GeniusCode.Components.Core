using System;

namespace GeniusCode.Components.Caching
{

    public static class ICacheAdapterExtensions
    {
        public static T GetOrCreate<TKey, T>(this ICacheAdapter<TKey, T> input, TKey key, Func<T> creator, Func<T, TKey> keyCreator)
        {
            return GetOrCreate(input, key, creator, keyCreator, null);
        }
        public static T GetOrCreate<TKey, T>(this ICacheAdapter<TKey, T> input, TKey key, Func<T> creator, Func<T, TKey> keyCreator, Func<T, bool> cachePredicate)
        {
            if (creator == null) throw new ArgumentNullException("creator");


            T result;

            if (cachePredicate == null)
                cachePredicate = a => true;

            if (input.CacheContainsItem(key))
                result = input.GetCachedItem(key);
            else
            {
                result = creator();
                // cache if predicate succeeds...
                if (cachePredicate(result))
                    input.CacheObject(keyCreator(result), result);
            }

            return result;
        }
    }

}
