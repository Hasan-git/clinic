using System;
using System.Web.Caching;

namespace Clinic.Common.Core.Caching
{
    public interface IAppCache
    {
        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs. Expired after 1440 min. (1 day)
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        void Add<T>(T o, string key);


        T GetOrAdd<T>(string key, Func<T> activator);

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="minutes">Number of minutes after which it expires </param>
        void Add<T>(T o, string key, int minutes);


        void Add<T>(T o, string key, int minutes, CacheItemRemovedCallback onCacheRemove);

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        void Clear(string key);

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <param name="value">Cached value. Default(T) if
        /// item doesn't exist.</param>
        /// <returns>Cached item as type</returns>
        bool TryGet<T>(string key, out T value);

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        T Get<T>(string key) where T : class;

        T[] GetAll<T>() where T : class;

    }
}