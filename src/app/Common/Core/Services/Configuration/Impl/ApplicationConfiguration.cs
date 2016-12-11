﻿using System.Configuration;

namespace Clinic.Common.Core.Services.Configuration.Impl
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string GetSetting(string settingName)
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];
            return settingValue;
        }

        public object GetSection(string sectionName)
        {
            var section = ConfigurationManager.GetSection(sectionName);
            return section;
        }
    }
}
