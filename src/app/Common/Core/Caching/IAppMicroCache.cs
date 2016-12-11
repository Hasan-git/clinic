using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Clinic.Common.Core.Caching
{
    public interface IAppMicroCache
    {
        void Clear();
        void Remove(string key);
        void Remove(string key, bool startsWith);

        void Update<TValue>(string key, Func<TValue> activator, bool forceCache = false);
        TValue GetOrAdd<TValue>(string key, Func<TValue> activator, bool forceCache = false);
        TValue GetOrAdd<TValue>(string key, bool shortLived, Func<TValue> activator, bool forceCache = false);
        TValue GetOrAdd<TValue>(string key, int timeInSeconds, Func<TValue> activator, bool forceCache = false);
        TValue GetOrAdd<TValue>(string key, int timeInSeconds, Func<TValue> activator, bool forceCache, CacheEntryRemovedCallback onCacheRemove = null);
        KeyValuePair<string, object>[] GetAll();

        bool DisableCaching { get; } 
    }
}