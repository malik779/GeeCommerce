using FluentMigrator.Runner.Initialization;
using Gee.Core.Enum;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
using Gee.Core.BaseInfrastructure.Config;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Gee.Core.BaseInfrastructure.DataProviders.Certificate;

namespace Gee.Core.BaseInfrastructure.DataProviders
{
    public partial class DataConfig : IConfig, IConnectionStringAccessor
    {
        /// <summary>
        /// Gets or sets a connection string
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a data provider
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DataProviderType DataProvider { get; set; } = DataProviderType.SqlServer;

        /// <summary>
        /// Gets or sets the wait time (in seconds) before terminating the attempt to execute a command and generating an error.
        /// By default, timeout isn't set and a default value for the current provider used.
        /// Set 0 to use infinite timeout.
        /// </summary>
        public int? SQLCommandTimeout { get; set; } = null;

        /// <summary>
        /// Gets or sets a value that indicates whether to add NoLock hint to SELECT statements (Reltates to SQL Server only)
        /// </summary>
        public bool WithNoLock { get; set; } = false;

        /// <summary>
        /// Gets a section name to load configuration
        /// </summary>
        [JsonIgnore]
        public string Name => nameof(ConfigurationManager.ConnectionStrings);

        /// <summary>
        /// Gets an order of configuration
        /// </summary>
        /// <returns>Order</returns>
        public int GetOrder() => 0; //display first

        public List<Client>? Clients { get; set; } = new List<Client>();
        public List<BaseClientResources>? ApiScopes { get; set; } = new List<BaseClientResources>();
        public List<BaseClientResources>? ApiResources { get; set; } = new List<BaseClientResources>();

        public CertificateDetail CertificateDetail { get; set; } = new CertificateDetail();
    }
    public partial class IdentityClientConfig : IConfig
    {
        [JsonIgnore]
        public string Name => "IdentityClients";

        /// <summary>
        /// Gets an order of configuration
        /// </summary>
        /// <returns>Order</returns>
        public int GetOrder() => 2; //display first

        public List<Client>? Clients { get; set; } = new List<Client>();
        public List<BaseClientResources>? ApiScopes { get; set; } = new List<BaseClientResources>();
        public List<BaseClientResources>? ApiResources { get; set; } = new List<BaseClientResources>();
    }
}
