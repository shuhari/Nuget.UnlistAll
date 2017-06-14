using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Nuget.UnlistAll.Models
{
    public class NugetParams
    {
        public NugetParams(string packageId, string apiKey)
        {
            this.PackageId = packageId;
            this.ApiKey = apiKey;
        }

        [Required(ErrorMessage = "Package ID should not be empty")]
        public string PackageId { get; private set; }

        [Required(ErrorMessage = "API Key should not be empty")]
        public string ApiKey { get; private set; }

        public void Validate()
        {
            var ctx = new ValidationContext(this);
            Validator.ValidateObject(this, ctx);
        }

        public static NugetParams Load()
        {
            var config = LoadConfig();
            var packageId = GetAppSetting(config, "nuget.packageId");
            var apiKey = GetAppSetting(config, "nuget.apiKey");
            return new NugetParams(packageId, apiKey);
        }

        public void Save()
        {
            var config = LoadConfig();
            SetAppSetting(config, "nuget.packageId", PackageId);
            SetAppSetting(config, "nuget.apiKey", ApiKey);
            config.Save();
        }

        private static Configuration LoadConfig()
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private static string GetAppSetting(Configuration config, string key)
        {
           var setting = config.AppSettings.Settings[key];
            return setting != null ? setting.Value : "";
        }

        private static void SetAppSetting(Configuration config, string key, string value)
        {
            var setting = config.AppSettings.Settings[key];
            if (setting != null)
                setting.Value = value;
            else
                config.AppSettings.Settings.Add(key, value);
        }
    }
}
