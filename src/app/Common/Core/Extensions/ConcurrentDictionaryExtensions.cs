using System;
using System.Collections.Concurrent;

namespace Clinic.Common.Core.Extensions
{
    public static class ConcurrentDictionaryExtensions
    {
        public static V GetOrAddSafe<T, V>(this ConcurrentDictionary<T, Lazy<V>> dictionary, T key, Func<T, V> valueFactory)
        {
            Lazy<V> lazy = dictionary.GetOrAdd(key, new Lazy<V>(() => valueFactory(key)));
            return lazy.Value;
        }

        public static V AddOrUpdateSafe<T, V>(this ConcurrentDictionary<T, Lazy<V>> dictionary, T key, Func<T, V> addValueFactory, Func<T, V, V> updateValueFactory)
        {
            Lazy<V> lazy = dictionary.AddOrUpdate(key,
                new Lazy<V>(() => addValueFactory(key)),
                (k, oldValue) => new Lazy<V>(() => updateValueFactory(k, oldValue.Value)));
            return lazy.Value;
        }
    }
}
