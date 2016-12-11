using System;
using System.Threading;

namespace Clinic.Common.Core.Enumerations
{
    /// <summary>
    /// An enumeration that handles mutliple locales based on
    /// Thread.CurrentThread.CurrentUICulture setting.
    /// </summary>
    [Serializable]
    public abstract class LocalizedEnumeration : Enumeration
    {
        public string[] _displayNames; // keep it public for Json Serialization issue
        protected string[] _supportedCultures = new[] { "en" };

        protected LocalizedEnumeration()
        {
            SetCultures();
        }

        protected LocalizedEnumeration(int value, string displayName)
            : base(value, displayName)
        {
            SetCultures();
            _displayNames = new[] { displayName };
        }
        protected LocalizedEnumeration(int value, params string[] displayNames)
            : this(value, "")
        {
            if (displayNames.Length != _supportedCultures.Length)
                throw new ArgumentOutOfRangeException("displayNames", "Display names should match the number of supported cultures");

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
