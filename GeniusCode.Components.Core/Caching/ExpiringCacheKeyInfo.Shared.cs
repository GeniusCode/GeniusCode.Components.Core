using System;

namespace GeniusCode.Components.Caching
{
    public class ExpiringCacheKeyInfo
    {
        public string Key { get; private set; }
        public TimeSpan? SlidingExpiration { get; private set; }
        public string[] DependancyKeys { get; private set; }
        public  DateTime? AbsoluteExpiration { get; private set; }

        #region constructors
        /// <summary>
        /// Full Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="dependencyKeys"></param>
        /// <param name="absoluteExpiraion"></param>
        public ExpiringCacheKeyInfo(string key, TimeSpan? slidingExpiration, string[] dependencyKeys, DateTime? absoluteExpiraion)
        {
            Key = key;
            SlidingExpiration = slidingExpiration;
            DependancyKeys = dependencyKeys;
            AbsoluteExpiration = absoluteExpiraion;
        }
        /// <summary>
        /// Used for only a sliding expiration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="slidingExpiration"></param>
        public ExpiringCacheKeyInfo(string key, TimeSpan? slidingExpiration)
            : this(key, slidingExpiration, null)
        {
        }
        /// <summary>
        /// used for only dependency keys
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dependencyKeys"></param>
        public ExpiringCacheKeyInfo(string key, string[] dependencyKeys)
            : this(key, TimeSpan.Zero, dependencyKeys)
        {
        }

        /// <summary>
        /// No absolute Expiration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="dependencyKeys"></param>
        public ExpiringCacheKeyInfo(string key, Nullable<TimeSpan> slidingExpiration, string[] dependencyKeys)
            : this(key, slidingExpiration, dependencyKeys, null)
        {
            AbsoluteExpiration = null;
        }
        #endregion


                                           

    }
}
