using System;
using System.Collections.Generic;

namespace Clinic.Common.Core.Services.Configuration.Impl
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly IApplicationConfiguration _settings;

        public ConfigurationReader(IApplicationConfiguration settings)
        {
            _settings = settings;
        }

        public string GetRequiredSetting(string key)
        {
            var setting = _settings.GetSetting(key);

            if (setting != null) return setting;

            var message = string.Format("The application setting '{0}' does not exist in the application configuration file.", key);
            throw new ApplicationException(message);
        }

        public int GetRequiredIntegerSetting(string key)
        {
            var settingString = GetRequiredSetting(key);

            int setting;
            var isInteger = int.TryParse(settingString, out setting);

            if (isInteger) return setting;

            const string template = "The value for setting '{0}' ('{1}') is not an integer";
            throw new ApplicationException(string.Format(template, key, settingString));
        }

        public bool GetRequiredBooleanSetting(string key)
        {
            var settingString = GetRequiredSetting(key);

            bool setting;
            var isBoolean = bool.TryParse(settingString, out setting);

            if (isBoolean) return setting;

            const string template = "The value for setting '{0}' ('{1}') is not a boolean";
            throw new ApplicationException(string.Format(template, key, settingString));
        }

        public IEnumerable<string> GetStringArray(string key)
        {
            var setting = _settings.GetSetting(key);

            if (setting == null) yield break;

            IEnumerable<string> rawSettings = setting.Split(',');

            foreach (var rawSetting in rawSettings)
            {
                yield return rawSetting.Trim();
            }
        }

        public bool? GetOptionalBooleanSetting(string key)
        {
            var settingString = GetOptionalSetting(key);

            bool setting;
            var isBoolean = bool.TryParse(settingString, out setting);
            var settingValue = !isBoolean ? null : new bool?(setting);
            return settingValue;
        }

        public string GetOptionalSetting(string key)
        {
            var setting = _settings.GetSetting(key);
            return setting;
        }
    }

}
