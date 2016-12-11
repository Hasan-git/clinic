using System.Collections.Generic;

namespace Clinic.Common.Core.Services.Configuration
{
    public interface IApplicationSettings
    {
        int GetServiceSleepTime();
        string SmtpServer { get; set; }
        int SmtpServerPort { get; set; }
        string GetSmtpUsername();
        string GetSmtpPassword();
        bool GetSmtpAuthenticationNecessary();
        IEnumerable<string> GetMappingAssemblies();
        bool GetShowSql();
        string GetServiceAgentFactory();
        bool GetSmtpServerRequireSsl();
    }
}
