using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clinic.Common.Core.Enumerations
{
    public static class EnumerationHelper
    {
        public static IEnumerable<Enumeration> GetAll(Type enumerationType)
        {
            FieldInfo[] fields = enumerationType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<Enumeration>();
        }

        public static Enumeration FromValue(Type enumerationType, int enumerationValue)
        {
            return GetAll(enumerationType).Single(e => e.Value == enumerationValue);
        }

        public static Enumeration FromValueOrDefault(Type enumerationType, int enumerationValue)
        {
            return GetAll(enumerationType).SingleOrDefault(e => e.Value == enumerationValue);
        }
        public static Enumeration FromKeyOrDefault(Type enumerationType, string key)
        {
            return GetAll(enumerationType).Cast<KeyedEnumeration>().SingleOrDefault(e => e.Key == key);
        }
        public static Enumeration FromDisplayNameOrDefault(Type enumerationType, string displayName)
        {
            return GetAll(enumerationType).SingleOrDefault(e => e.DisplayName == displayName);
        }
    }

}
