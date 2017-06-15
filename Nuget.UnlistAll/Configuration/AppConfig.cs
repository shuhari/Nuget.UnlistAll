using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Nuget.UnlistAll.Resources;
using SysConfiguration = System.Configuration.Configuration;

namespace Nuget.UnlistAll.Configuration
{
    /// <summary>
    /// Application configuration info
    /// </summary>
    public class AppConfig
    {
        public AppConfig(string packageId, string apiKey)
        {
            this.PackageId = packageId;
            this.ApiKey = apiKey;
        }

        [Required(ErrorMessage = Strings.PackageIdRequired)]
        public string PackageId { get; private set; }

        [Required(ErrorMessage = Strings.ApiKeyRequired)]
        public string ApiKey { get; private set; }

        public void Validate()
        {
            var ctx = new ValidationContext(this);
            Validator.ValidateObject(this, ctx);
        }

        public static AppConfig Load()
        {
            var config = LoadConfig();
            var packageId = GetAppSetting(config, "nuget.packageId");
            var apiKey = GetAppSetting(config, "nuget.apiKey");
            return new AppConfig(packageId, apiKey);
        }

        public void Save()
        {
            var config = LoadConfig();
            SetAppSetting(config, "nuget.packageId", PackageId);
            SetAppSetting(config, "nuget.apiKey", ApiKey);
            config.Save();
        }

        private static SysConfiguration LoadConfig()
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private static string GetAppSetting(SysConfiguration config, string key)
        {
           var setting = config.AppSettings.Settings[key];
            return setting != null ? setting.Value : "";
        }

        private static void SetAppSetting(SysConfiguration config, string key, string value)
        {
            var setting = config.AppSettings.Settings[key];
            if (setting != null)
                setting.Value = value;
            else
                config.AppSettings.Settings.Add(key, value);
        }
    }
}
