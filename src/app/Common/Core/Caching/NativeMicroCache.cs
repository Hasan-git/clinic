using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;

namespace Clinic.Common.Core.Caching
{
    public class NativeMicroCache : IAppMicroCache
    {
        private ReaderWriterLockSlim _lock;
        private static readonly ObjectCache _cache = MemoryCache.Default;

        public NativeMicroCache()
        {
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }
        public void Update<TValue>(string key, Func<TValue> activator, bool forceCache = false)
        {
            Remove(key);
            GetOrAdd(key, activator, forceCache);
        }

        public TValue GetOrAdd<TValue>(string key, Func<TValue> activator, bool forceCache = false)
        {
            return GetOrAdd(key, true, activator, forceCache);
        }

        public TValue GetOrAdd<TValue>(string key, bool shortLived, Func<TValue> activator, bool forceCache = false)
        {
            return GetOrAdd<TValue>(key, (shortLived ? 1 * 60 : 10 * 60), activator, forceCache);
        }

        public TValue GetOrAdd<TValue>(string key, int timeInSeconds, Func<TValue> activator, bool forceCache = false)
        {
            return GetOrAdd<TValue>(key, timeInSeconds, activator, forceCache, null);
        }

        public TValue GetOrAdd<TValue>(string key, int timeInSeconds, Func<TValue> activator, bool forceCache, CacheEntryRemovedCallback onCacheRemove)
        {
            bool success;
            LazyLock lazy = null;
            _lock.EnterReadLock();

            try
            {
                lazy = (LazyLock)_cache.Get(key);
                success = lazy != null;
            }
            finally
            {
                _lock.ExitReadLock();
            }

            if (!success)
            {
                _lock.EnterWriteLock();

                try
                {
                    lazy = new LazyLock();
                    if (!DisableCaching || forceCache)
                    {
                        //var utcExpiry = DateTime.UtcNow.AddSeconds(timeInSeconds);
                        var policy = new CacheItemPolicy();
                        policy.Priority = System.Runtime.Caching.CacheItemPriority.NotRemovable;
                        policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(timeInSeconds);
                        policy.RemovedCallback = onCacheRemove;
                        //policy.ChangeMonitors.Add(new HostFileChangeMonitor(FilePath));

                        _cache.Add(key, lazy, policy);
                    }
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            return lazy.Get(activator);
        }

        public KeyValuePair<string, object>[] GetAll()
        {
            return _cache.Select(x => x).ToArray();
        }

        public void Clear()
        {
            foreach (var key in _cache.Select(kvp => kvp.Key).ToList())
            {
                _cache.Remove(key);
            }
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Remove(string key, bool startsWith)
        {
            if (startsWith)
                foreach (var itemKey in _cache.Select(kvp => kvp.Key).Where(x => x.StartsWith(key)).ToList())
                {
                    _cache.Remove(itemKey);
                }
            else
            {
                _cache.Remove(key);
            }
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
