using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace Clinic.Common.Core.Caching
{
    public class AppCache : IAppCache
    {
        private readonly Cache _cahce;

        public AppCache()
        {
            _cahce = HttpRuntime.Cache;
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public void Add<T>(T o, string key)
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            Add<T>(o, key, 1440);
        }

        public T GetOrAdd<T>(string key, Func<T> activator)
        {
            var minutes = 5;
            var obj = activator();
            if (obj == null)
                throw new Exception(string.Format("Can't insert a null object [Key = '{0}'] into the cache", key));
            var item = new CachedItem() { Value = obj, Type = obj.GetType() };
            _cahce.Insert(
                key,
                item,
                null,
                DateTime.Now.AddMinutes(minutes),
                Cache.NoSlidingExpiration);

            return obj;
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="minutes">Number of minutes after which it expires </param>
        public void Add<T>(T o, string key, int minutes)
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            // In this example, I want an absolute
            // timeout so changes will always be reflected
            // at that time. Hence, the NoSlidingExpiration.
            if (o == null)
                throw new Exception(string.Format("Can't insert a null object [Key = '{0}'] into the cache", key));
            var item = new CachedItem() { Value = o, Type = o.GetType() };
            _cahce.Insert(
                key,
                item,
                null,
                DateTime.Now.AddMinutes(minutes),
                Cache.NoSlidingExpiration);
        }

        public void Add<T>(T o, string key, int minutes, CacheItemRemovedCallback onCacheRemove)
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            // In this example, I want an absolute
            // timeout so changes will always be reflected
            // at that time. Hence, the NoSlidingExpiration.
            if (o == null)
                throw new Exception(string.Format("Can't insert a null object [Key = '{0}'] into the cache", key));
            var item = new CachedItem() { Value = o, Type = o.GetType() };
            _cahce.Insert(
                key,
                item,
                null,
                DateTime.Now.AddMinutes(minutes),
                Cache.NoSlidingExpiration, CacheItemPriority.Normal, onCacheRemove);
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Clear(string key)
        {
            _cahce.Remove(key);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return _cahce[key] != null;
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <param name="value">Cached value. Default(T) if
        /// item doesn't exist.</param>
        /// <returns>Cached item as type</returns>
        public bool TryGet<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }
                var item = (CachedItem)_cahce[key];
                value = (T)item.Value;
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public T Get<T>(string key) where T : class
        {
            try
            {
                var item = (CachedItem)_cahce[key];
                return (T)item.Value;
            }
            catch
            {
                return null;
            }
        }


        public T[] GetAll<T>() where T : class
        {
            List<T> items = new List<T>();

            IDictionaryEnumerator enumerator = _cahce.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (((CachedItem)enumerator.Value).Type == typeof(T))
                {
                    items.Add((T)((CachedItem)enumerator.Value).Value);
                }
            }

            return items.ToArray();
        }
    }
}
