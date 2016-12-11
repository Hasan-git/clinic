using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;

namespace Clinic.Common.Core.Caching
{
    public class AppMicroCache : IAppMicroCache
    {
        private readonly MicroCache<string> _shortCache;
        private readonly MicroCache<string> _longCache;

        public AppMicroCache()
        {
            _shortCache = new MicroCache<string>(TimeSpan.FromMinutes(2));
            _longCache = new MicroCache<string>(TimeSpan.FromMinutes(10));
        }

        public void Update<TValue>(string key, Func<TValue> activator, bool forceCache = false)
        {
            //GetOrAdd(key, activator, forceCache);
        }

        public TValue GetOrAdd<TValue>(string key, Func<TValue> activator, bool forceCache = false)
        {
            return GetOrAdd(key, true, activator, forceCache);
        }

        public TValue GetOrAdd<TValue>(string key, bool shortLived, Func<TValue> activator, bool forceCache = false)
        {
            return GetOrAdd<TValue>(key, (shortLived ? 5 * 60 : 120 * 60), activator, forceCache);
        }

        public TValue GetOrAdd<TValue>(string key, int timeInSeconds, Func<TValue> activator, bool forceCache = false)
        {
            return GetOrAdd<TValue>(key, timeInSeconds, activator, forceCache, null);
        }

        public TValue GetOrAdd<TValue>(string key, int timeInSeconds, Func<TValue> activator, bool forceCache = false, CacheEntryRemovedCallback onCacheRemove = null)
        {
            if (DisableCaching && !forceCache) return activator();

            //if (shortLived)
            return _shortCache.GetOrAdd(key, activator);
            //else
            //    return _longCache.GetOrAdd(key, activator);

        }
        public KeyValuePair<string, object>[] GetAll()
        {
            return null;
        }

        public void Clear()
        {
            _shortCache.Clear();
            _longCache.Clear();
        }

        public void Remove(string key)
        {
            Remove(key, false);
        }

        public void Remove(string key, bool startsWith)
        {
            if (_shortCache.Contains(key))
                _shortCache.Clear(key);
            else
                _longCache.Clear(key);
        }

        public bool DisableCaching
        {
            get
            {
                var config = ConfigurationManager.AppSettings["DisableCaching"];
                if (string.IsNullOrEmpty(config)) return false;
                return bool.Parse(config);
            }
        }
    }
}
