﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure.Config
{
    public partial interface IConfig
    {
        /// <summary>
        /// Gets a section name to load configuration
        /// </summary>
        [JsonIgnore]
        
        string Name => GetType().Name;

        /// <summary>
        /// Gets an order of configuration
        /// </summary>
        /// <returns>Order</returns>
        public int GetOrder() => 1;
    }
}
