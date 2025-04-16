using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure
{
    public static partial class DefaultConfigurationVariables
    {

        /// <summary>
        /// Gets the path to file that contains app settings
        /// </summary>
        public static string AppSettingsFilePath => "appsettings.json";

        /// <summary>
        /// Gets the path to file that contains app settings for specific hosting environment
        /// </summary>
        /// <remarks>0 - Environment name</remarks>
        public static string AppSettingsEnvironmentFilePath => "App_Data/appsettings.{0}.json";

        public static string ObsoleteFilePath => "~/App_Data/Settings.txt";

        /// <summary>
        /// Gets a path to the file that contains data settings
        /// </summary>
        public static string DataSettingFilePath => "~/App_Data/dataSettings.json";
    }

}
