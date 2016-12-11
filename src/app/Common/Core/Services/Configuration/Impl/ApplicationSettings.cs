using System.Collections.Generic;

namespace Clinic.Common.Core.Services.Configuration.Impl
{
    public class ApplicationSettings : IApplicationSettings
    {
        protected readonly IConfigurationReader _configurationReader;
        private int _smtpServerPort;
        private string _smtpServer;

        public ApplicationSettings(IConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
        }

        public int GetServiceSleepTime()
        {
            int setting = _configurationReader.GetRequiredIntegerSetting("ServiceSleepTime");
            return setting;
        }

        public string SmtpServer
        {
            get
            {
                if (string.IsNullOrEmpty(_smtpServer))
                {
                    _smtpServer = _configurationReader.GetRequiredSetting("SmtpServer");
                }
                return _smtpServer;
            }
            set { _smtpServer = value; }
        }

        public bool GetSmtpAuthenticationNecessary()
        {
            var setting = _configurationReader.GetRequiredBooleanSetting("SmtpAuthenticationNecessary");
            return setting;
        }

        public IEnumerable<string> GetMappingAssemblies()
        {
            var settings = _configurationReader.GetStringArray("MappingAssemblies");
            return settings;
        }

        public bool GetShowSql()
        {
            var showSql = _configurationReader.GetOptionalBooleanSetting("ShowSql") ?? false;
            return showSql;
        }

        public string GetServiceAgentFactory()
        {
            var factory = _configurationReader.GetRequiredSetting("ServiceAgentFactory");
            return factory;
        }

        public bool GetSmtpServerRequireSsl()
        {
            var requireSsl = _configurationReader.GetOptionalBooleanSetting("SmtpServerRequireSsl") ?? false;
            return requireSsl;
        }

        public int SmtpServerPort
        {
            get
            {
                if (_smtpServerPort == 0)
                {
                    _smtpServerPort = _configurationReader.GetRequiredIntegerSetting("SmtpServerPort");
                }
                return _smtpServerPort;
            }
            set { _smtpServerPort = value; }
        }

        public string GetSmtpUsername()
        {
            var setting = _configurationReader.GetRequiredSetting("SmtpUsername");
            return setting;
        }

        public string GetSmtpPassword()
        {
            var setting = _configurationReader.GetRequiredSetting("SmtpPassword");
            return setting;
        }
    }

}
