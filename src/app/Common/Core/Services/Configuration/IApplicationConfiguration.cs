
namespace Clinic.Common.Core.Services.Configuration
{
    public interface IApplicationConfiguration
    {
        string GetSetting(string settingName);
        object GetSection(string sectionName);
    }
}
