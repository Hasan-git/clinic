using System.Collections;
using System.Collections.Generic;

namespace Clinic.Common.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns true if this objects is 'in' this list
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="listOfObjects"></param>
        /// <returns></returns>
        public static bool IsIn(this object obj, IList listOfObjects)
        {
            return listOfObjects.Contains(obj);
        }
        public static bool IsIn(this long obj, IList<long> listOfObjects)
        {
            return listOfObjects.Contains(obj);
        }
        public static bool IsIn(this int obj, IList<int> listOfObjects)
        {
            return listOfObjects.Contains(obj);
        }
        public static bool IsIn(this string obj, IList<string> listOfObjects)
        {
            return listOfObjects.Contains(obj);
        }

        public static string ToNullSafeString(this object value)
        {
            return value == null ? string.Empty : value.ToString();
        }

    }
}
