using Gee.Core.BaseInfrastructure.Config;
using Gee.Core.BaseInfrastructure.DataProviders.Certificate;
using Gee.Core.BaseInfrastructure.FileProvider;
using Gee.Core.BaseInfrastructure.Helpers;
using Gee.Core.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure.DataProviders
{
    public partial class DataSettingsManager
    {
        #region Fields

        /// <summary>
        /// Gets a cached value indicating whether the database is installed. We need this value invariable during installation process
        /// </summary>
        public static bool? _databaseIsInstalled;
        public static bool _isDBcontext;
        protected static bool _iseEnableIdentityServer;
        #endregion

        #region Utilities

        /// <summary>
        /// Gets data settings from the old txt file (Settings.txt)
        /// </summary>
        /// <param name="data">Old txt file data</param>
        /// <returns>Data settings</returns>
        protected static DataConfig LoadDataSettingsFromOldTxtFile(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var dataSettings = new DataConfig();
            using var reader = new StringReader(data);
            string settingsLine;
            while (!string.IsNullOrEmpty(settingsLine = reader.ReadLine()??""))
            {
                var separatorIndex = settingsLine.IndexOf(':');
                if (separatorIndex == -1)
                    continue;

                var key = settingsLine[0..separatorIndex].Trim();
                var value = settingsLine[(separatorIndex + 1)..].Trim();

                switch (key)
                {
                    case "DataProvider":
                        dataSettings.DataProvider = Enum(value, true, out DataProviderType providerType) ? DataProviderType.Unknown : providerType;
                        continue;
                    case "DataConnectionString":
                        dataSettings.ConnectionString = value;
                        continue;
                    case "SQLCommandTimeout":
                        //If parsing isn't successful, we set a negative timeout, that means the current provider will use a default value
                        dataSettings.SQLCommandTimeout = int.TryParse(value, out var timeout) ? timeout : -1;
                        continue;
                    default:
                        break;
                }
            }

            return dataSettings;
        }

        private static bool Enum(string value, bool v, out DataProviderType providerType)
        {
            switch (value) {
                case "sqlserver":
                    providerType = DataProviderType.SqlServer;
                    return true;
                case "postgresql":
                    providerType = DataProviderType.PostgreSQL;
                    return true;
                case "mysql":
                    providerType = DataProviderType.MySql;
                    return true;
                default:
                    providerType = DataProviderType.Unknown;
                    return false;
                    
            }
        }

        /// <summary>
        /// Gets data settings from the old json file (dataSettings.json)
        /// </summary>
        /// <param name="data">Old json file data</param>
        /// <returns>Data settings</returns>
        protected static DataConfig LoadDataSettingsFromOldJsonFile(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var jsonDataSettings = JsonConvert.DeserializeAnonymousType(data,
                new { DataConnectionString = "", DataProvider = DataProviderType.SqlServer, SQLCommandTimeout = "" });
            var dataSettings = new DataConfig
            {
                ConnectionString = jsonDataSettings.DataConnectionString,
                DataProvider = jsonDataSettings.DataProvider,
                SQLCommandTimeout = int.TryParse(jsonDataSettings.SQLCommandTimeout, out var result) ? result : null
            };

            return dataSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load data settings
        /// </summary>
        /// <param name="fileProvider">File provider</param>
        /// <param name="reload">Force loading settings from disk</param>
        /// <returns>Data settings</returns>
        public static DataConfig LoadSettings(IGenericFileProvider fileProvider = null, bool reload = false)
        {
            if (!reload && Singleton<DataConfig>.Instance is not null)
                return Singleton<DataConfig>.Instance;

            //backward compatibility
            fileProvider ??= CommonHelper.DefaultFileProvider;
            var filePath_json = fileProvider.MapPath(DefaultConfigurationVariables.DataSettingFilePath);
            
            if (fileProvider.FileExists(filePath_json))
            {
                var dataSettings = fileProvider.FileExists(filePath_json)
                    ? LoadDataSettingsFromOldJsonFile(fileProvider.ReadAllText(filePath_json, Encoding.UTF8))
                      : new DataConfig();

                fileProvider.DeleteFile(filePath_json);

                AppSettingHelper.SaveAppSetting(new List<IConfig> { dataSettings }, fileProvider);
                Singleton<DataConfig>.Instance = dataSettings;
            }
            else
            {
                
                var DataConfig= Singleton<AppSettingConfig>.Instance.Get<DataConfig>();
                var clients=Singleton<AppSettingConfig>.Instance.Get<IdentityClientConfig>();
                DataConfig.Clients = clients==null?new List<Client>():clients?.Clients;
                DataConfig.ApiScopes = clients == null ? new List<BaseClientResources>() : clients?.ApiScopes;
                DataConfig.ApiResources = clients == null ? new List<BaseClientResources>() : clients?.ApiResources;
                Singleton<DataConfig>.Instance = DataConfig;
            }
            IsSetDbContextUse();
            return Singleton<DataConfig>.Instance;
        }

        /// <summary>
        /// Save data settings
        /// </summary>
        /// <param name="dataSettings">Data settings</param>
        /// <param name="fileProvider">File provider</param>
        //public static void SaveSettings(DataConfig dataSettings, INopFileProvider fileProvider)
        //{
        //    AppSettingsHelper.SaveAppSettings(new List<IConfig> { dataSettings }, fileProvider);
        //    LoadSettings(fileProvider, reload: true);
        //}

        /// <summary>
        /// Gets a value indicating whether database is already installed
        /// </summary>
        public static bool IsDatabaseInstalled()
        {
            _databaseIsInstalled ??= !string.IsNullOrEmpty(LoadSettings()?.ConnectionString);

            return _databaseIsInstalled.Value;
        }
        public static void IsSetDbContextUse()
        {
            _isDBcontext = true;
        }

        /// <summary>
        /// Gets the command execution timeout.
        /// </summary>
        /// <value>
        /// Number of seconds. Negative timeout value means that a default timeout will be used. 0 timeout value corresponds to infinite timeout.
        /// </value>
        public static int GetSqlCommandTimeout()
        {
            return LoadSettings()?.SQLCommandTimeout ?? -1;
        }

        /// <summary>
        /// Gets a value that indicates whether to add NoLock hint to SELECT statements (applies only to SQL Server, otherwise returns false)
        /// </summary>
        public static bool UseNoLock()
        {
            var settings = LoadSettings();

            if (settings is null)
                return false;

            return settings.DataProvider == DataProviderType.SqlServer && settings.WithNoLock;
        }
        public static void SaveCertificateDetail(CertificateDetail certificateDetail)
        {
            var DataConfig = Singleton<AppSettingConfig>.Instance.Get<DataConfig>();
            DataConfig.CertificateDetail= certificateDetail;
            Singleton<DataConfig>.Instance = DataConfig;
        }
        #endregion
    }
}
