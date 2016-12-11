using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clinic.Common.Core.Enumerations
{
    [Serializable]
    public abstract class Enumeration : IEquatable<Enumeration>, IComparable
    {
        public int _value;
        public string _displayName;
        private static readonly Dictionary<Type, List<object>> AllEnums = new Dictionary<Type, List<object>>();
        private static object _lock = new object();

        protected Enumeration()
        {
        }

        protected Enumeration(int value, string displayName)
        {
            _value = value;
            _displayName = displayName;
        }

        public int Value
        {
            get { return _value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            lock (_lock)
            {
                var type = typeof(T);
                if (!AllEnums.ContainsKey(type))
                    AllEnums.Add(type, new List<object>());
                if (AllEnums[type].Count == 0)
                {
                    var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

                    foreach (var info in fields)
                    {
                        var instance = new T();
                        var locatedValue = info.GetValue(instance) as T;

                        if (locatedValue != null)
                        {
                            AllEnums[type].Add(locatedValue);
                        }
                    }
                }
                return AllEnums[type].Cast<T>().ToList();
            }
        }

        public static IEnumerable GetAll(Type type)
        {
            if (!AllEnums.ContainsKey(type))
                AllEnums.Add(type, new List<object>());
            if (AllEnums[type].Count == 0)
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

                foreach (var info in fields)
                {
                    object instance = Activator.CreateInstance(type);
                    AllEnums[type].Add(info.GetValue(instance));
                }
            }
            return AllEnums[type];
        }

        public bool Equals(Enumeration other)
        {
            if (ReferenceEquals(null, other)) return false;

            var typeMatches = GetType() == other.GetType();
            var valueMatches = _value.Equals(other.Value);

            return typeMatches && valueMatches;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (!(obj is Enumeration)) return false;
            return Equals(obj as Enumeration);
        }
        /// <summary>
        /// Equals operator.
        /// </summary>
        /// <param name="left">The left hand side.</param>
        /// <param name="right">The right hand side.</param>
        /// <returns><c>true</c> is <paramref name="left"/> is equal to <paramref name="right"/>; otherwise <c>false</c>.</returns>
        public static bool operator ==(Enumeration left, Enumeration right)
        {
            return ReferenceEquals(null, left) ?
                (ReferenceEquals(null, right) ? true : false)
                : left.Equals(right);

        }



        /// <summary>
        /// Not Equals operator.
        /// </summary>
        /// <param name="left">The left hand side.</param>
        /// <param name="right">The right hand side.</param>
        /// <returns><c>true</c> is <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise <c>false</c>.</returns>
        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !ReferenceEquals(null, left) && !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration, new()
        {
            var matchingItem = parse<T, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        protected static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }
        protected static T TryParse<T, K>(K value, string description, Func<T, bool> predicate, T defaultItem) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                if (defaultItem == null)
                    matchingItem = GetAll<T>().First();
                else
                    matchingItem = defaultItem;
            }
            return matchingItem;
        }

        public virtual int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration)other).Value);
        }

        public static T[] FromValues<T>(IEnumerable<int> values) where T : Enumeration, new()
        {
            return values.Count() == 0 ? new T[0] : values.Select(FromValue<T>).ToArray();
        }
    }
}
