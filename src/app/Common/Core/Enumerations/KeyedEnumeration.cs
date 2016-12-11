using System;
using System.Linq;

namespace Clinic.Common.Core.Enumerations
{
    [Serializable]
    public abstract class KeyedEnumeration : Enumeration
    {
        protected string _key;
        protected KeyedEnumeration()
        {
        }

        protected KeyedEnumeration(int value, string displayName)
            : base(value, displayName)
        {
        }
        protected KeyedEnumeration(int value, string key, string displayName)
            : base(value, displayName)
        {
            _key = key;
        }

        public string Key
        {
            get { return _key; }
        }

        public static T FromKey<T>(string key) where T : KeyedEnumeration, new()
        {
            var matchingItem = parse<T, string>(key, "key", item => item.Key.ToLower() == key.ToLower());
            return matchingItem;
        }
        public static T FromKeyTry<T>(string key, T defaultItem) where T : KeyedEnumeration, new()
        {
            var matchingItem = TryParse<T, string>(key, "key", item => string.Compare(item.Key, key, true) == 0, defaultItem);
            return matchingItem;
        }
        public static T[] FromKey<T>(string[] keys) where T : KeyedEnumeration, new()
        {
            return keys.Select(key => parse<T, string>(key, "key", item => string.Compare(item.Key, key, true) == 0)).ToArray();
        }
    }
}
