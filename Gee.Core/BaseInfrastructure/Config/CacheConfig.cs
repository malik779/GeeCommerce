using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure.Config
{
    public partial class CacheConfig : IConfig
    {
        /// <summary>
        /// Gets or sets the default cache time in minutes
        /// </summary>
        public int DefaultCacheTime { get; protected set; } = 60;

        /// <summary>
        /// Gets or sets whether to disable linq2db query cache
        /// </summary>
        public bool LinqDisableQueryCache { get; protected set; } = false;
    }
}
