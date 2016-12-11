using System;
using System.Collections.Generic;
using System.Threading;

namespace Clinic.Common.Core.Caching
{
    public sealed class MicroCache<TKey>
    {
        private IDictionary<TKey, LazyLock> _dictionary;
        private ReaderWriterLockSlim _lock;
        private Timer _timer;

        public MicroCache(TimeSpan period)
            : this(period, Comparer<TKey>.Default)
        {
        }

        public MicroCache(TimeSpan period, IComparer<TKey> comparer)
        {
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _dictionary = new SortedDictionary<TKey, LazyLock>(comparer);
            _timer = new Timer(state => Clear(), null, period, period);
        }

        public void Clear()
        {
            _lock.EnterWriteLock();

            try
            {
                _dictionary.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        internal void Clear(TKey key)
        {
            _lock.EnterWriteLock();

            try
            {
                _dictionary.Remove(key);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        internal bool Contains(TKey key)
        {
            _lock.EnterWriteLock();

            try
            {
                return _dictionary.ContainsKey(key);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Update<TValue>(TKey key, Func<TValue> activator)
        {
            _dictionary.Remove(key);
            GetOrAdd(key, activator);
        }

        public TValue GetOrAdd<TValue>(TKey key, Func<TValue> activator)
        {
            LazyLock lazy;
            bool success;

            _lock.EnterReadLock();

            try
            {
                success = _dictionary.TryGetValue(key, out lazy);
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
                    if (!_dictionary.TryGetValue(key, out lazy))
                    {
                        _dictionary[key] = lazy = new LazyLock();
                    }
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }

            return lazy.Get(activator);
        }


        //internal void RemoveAllStartsWith(TKey key, Func<KeyValuePair<TKey, MicroCache<TKey, LazyLock>>,int, bool> expression)
        //{
        //    _lock.EnterWriteLock();

        //    try
        //    {
        //        var removeList= _dictionary.Where(expression);
        //        foreach(var item in removeList)
        //        {
        //            _dictionary.Remove(item);
        //        }
        //    }
        //    finally
        //    {
        //        _lock.ExitWriteLock();
        //    }
        //}
    }
}
