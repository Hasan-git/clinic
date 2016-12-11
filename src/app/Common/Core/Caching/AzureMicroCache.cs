//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Runtime.Caching;
//using Mentis.AlMaknaz.Common.Core.Services;

//namespace Mentis.AlMaknaz.Common.Core.Caching
//{
//    public class AzureMicroCache : IAppMicroCache
//    {
//        private const string _cacheName = "default";
//        private const string _default = "default";
//        private static DataCache _cache;

//        public AzureMicroCache()
//        {
//            try
//            {
//                //DataCacheFactoryConfiguration config = new DataCacheFactoryConfiguration("default");
//                //DataCacheFactory cacheFactory = new DataCacheFactory(config);
//                //_cache = cacheFactory.GetDefaultCache();    

//                _cache = new DataCache(_cacheName);
//                _cache.CreateRegion(_default);
//            }
//            catch (Exception exception)
//            {
//                Logger.Error(this, "Error locating Azure Cache.", exception);
//            }
//        }
//        public void Update<TValue>(string key, Func<TValue> activator, bool forceCache = false)
//        {
//            //Logger.Error(this, "Updating cache item: " + key);
//            if (DisableCaching && !forceCache) return;
//            _cache.Put(key, activator(), new[] { new DataCacheTag(_default) }, _default);
//        }

//        public TValue GetOrAdd<TValue>(string key, Func<TValue> activator, bool forceCache = false)
//        {
//            //Logger.Error(this, "Select cache item: " + key);
//            return GetOrAdd(key, true, activator, forceCache);
//        }

//        public TValue GetOrAdd<TValue>(string key, bool shortLived, Func<TValue> activator, bool forceCache = false)
//        {
//            return GetOrAdd<TValue>(key, (shortLived ? 5 * 60 : 120 * 60), activator, forceCache);
//        }

//        public TValue GetOrAdd<TValue>(string key, int timeInSeconds, Func<TValue> activator, bool forceCache = false)
//        {
//            return GetOrAdd<TValue>(key, timeInSeconds, activator, forceCache, null);
//        }

//        public TValue GetOrAdd<TValue>(string key, int timeInSeconds, Func<TValue> activator, bool forceCache = false, CacheEntryRemovedCallback onCacheRemove = null)
//        {
//            var value = _cache.Get(key, _default);
//            if (value == null)
//            {
//                try
//                {
//                    value = activator();
//                }
//                catch
//                {
//                    return default(TValue);
//                }
//                if (value != null && (!DisableCaching || forceCache))
//                {
//                    var utcExpiry = TimeSpan.FromSeconds(timeInSeconds);
//                    _cache.Put(key, value, utcExpiry, new[] { new DataCacheTag(_default) }, _default);
//                }
//            }
//            return (TValue)value;
//        }

//        public void Clear()
//        {
//            _cache.Clear();
//        }

//        public void Remove(string key)
//        {
//            _cache.Remove(key, _default);
//        }

//        public void Remove(string key, bool startsWith)
//        {
//            if (startsWith)
//                foreach (var itemKey in _cache.GetObjectsByTag(new DataCacheTag(_default), _default).Select(kvp => kvp.Key).Where(x => x.StartsWith(key)).ToList())
//                {
//                    _cache.Remove(itemKey);
//                }
//            else
//            {
//                _cache.Remove(key);
//            }
//        }

//        public KeyValuePair<string, object>[] GetAll()
//        {
//            return _cache.GetObjectsByTag(new DataCacheTag(_default), _default).ToArray();
//        }

//        public bool DisableCaching
//        {
//            get
//            {
//                var config = ConfigurationManager.AppSettings["DisableCaching"];
//                if (string.IsNullOrEmpty(config)) return false;
//                return bool.Parse(config);
//            }
//        }
//    }
//}
