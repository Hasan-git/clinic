using System;
using System.Collections.Generic;
using System.Linq;

namespace Clinic.Common.Core.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// http://stackoverflow.com/questions/932300/c-sharp-ranking-of-objects-multiple-criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<U> Rank<T, TKey, U>
                                                    (
                                                        this IEnumerable<T> source,
                                                        Func<T, TKey> keySelector,
                                                        Func<T, int, U> selector
                                                    )
        {
            if (!source.Any())
            {
                yield break;
            }

            int itemCount = 0;
            T[] ordered = source.OrderBy(keySelector).ToArray();
            TKey previous = keySelector(ordered[0]);
            int rank = 1;
            foreach (T t in ordered)
            {
                itemCount += 1;
                TKey current = keySelector(t);
                if (!current.Equals(previous))
                {
                    rank = itemCount;
                }
                yield return selector(t, rank);
                previous = current;
            }
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> self, int size)
        {
            var items = new List<T>();
            foreach (var item in self)
            {
                items.Add(item);
                if (items.Count < size)
                    continue;
                yield return items;
                items.Clear();
            }
            yield return items;
        }        
    }
}
