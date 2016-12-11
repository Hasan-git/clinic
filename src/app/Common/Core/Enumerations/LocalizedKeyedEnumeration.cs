using System;
using System.Threading;

namespace Clinic.Common.Core.Enumerations
{
    [Serializable]
    public abstract class LocalizedKeyedEnumeration : KeyedEnumeration
    {
        //private readonly string _key;
        public string[] _displayNames; // keep it public for Json Serialization issue
        protected string[] _supportedCultures = new[] { "en" };

        protected LocalizedKeyedEnumeration()
            : this(-1, "", "")
        {
        }

        protected LocalizedKeyedEnumeration(int value, string key, string displayName)
            : base(value, displayName)
        {
            SetCultures();
            _displayNames = new[] { displayName };

            _key = key;
        }

        protected LocalizedKeyedEnumeration(int value, string displayName)
            : this(value, "", displayName)
        {
        }
        protected LocalizedKeyedEnumeration(int value, string key, params string[] displayNames)
            : this(value, "")
        {
            _key = key;
            _displayNames = displayNames;
        }

        /// <summary>
        /// Will display the name based on the current thread UI culture
        /// </summary>
        public new string DisplayName
        {
            get
            {
                var culture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                var index = Array.IndexOf(_supportedCultures, culture);
                if (index < 0)
                    return "";
                return _displayNames[index];
            }
        }

        public string[] DisplayNames
        {
            get { return _displayNames; }
        }

        public string[] SupportedCultures
        {
            get { return _supportedCultures; }
        }

        protected abstract void SetCultures();

        public override string ToString()
        {
            return DisplayName;
        }

    }
}
