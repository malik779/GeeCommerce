using Gee.Core.BaseInfrastructure.Config;
using Gee.Core.BaseInfrastructure.FileProvider;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure.Helpers
{
    public partial class AppSettingHelper
    {
        #region Fields

        protected static Dictionary<string, int> _configurationOrder;

        #endregion

        public static AppSettingConfig SaveAppSetting(IList<IConfig> configurations, IGenericFileProvider fileProvider, bool overwrite = true)
        {
            ArgumentNullException.ThrowIfNull(configurations);

            _configurationOrder ??= configurations.ToDictionary(config => config.Name, config => config.GetOrder());

            //create app settings
            var appSettings = Singleton<AppSettingConfig>.Instance ?? new AppSettingConfig();
            appSettings.Update(configurations);
            Singleton<AppSettingConfig>.Instance = appSettings;

            //create file if not exists
            var filePath = fileProvider.MapPath(DefaultConfigurationVariables.AppSettingsFilePath);
            var fileExists = fileProvider.FileExists(filePath);
            fileProvider.CreateFile(filePath);

            //get raw configuration parameters
            var configuration = JsonConvert.DeserializeObject<AppSettingConfig>(fileProvider.ReadAllText(filePath, Encoding.UTF8))
                                    ?.Configuration
                                ?? new();
            foreach (var config in configurations)
            {
                configuration[config.Name] = JToken.FromObject(config);
            }

            //sort configurations for display by order (e.g. data configuration with 0 will be the first)
            appSettings.Configuration = configuration
                .SelectMany(outConfig => _configurationOrder.Where(inConfig => inConfig.Key == outConfig.Key).DefaultIfEmpty(),
                    (outConfig, inConfig) => new { OutConfig = outConfig, InConfig = inConfig })
                .OrderBy(config => config.InConfig.Value)
                .Select(config => config.OutConfig)
                .ToDictionary(config => config.Key, config => config.Value);

            //save app settings to the file
            if (!fileExists || overwrite)
            {
                var text = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
                fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
            }

            return appSettings;
        }
    }
}
