﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.Enum
{
    public enum BackorderMode
    {
        /// <summary>
        /// No backorders
        /// </summary>
        NoBackorders = 0,

        /// <summary>
        /// Allow qty below 0
        /// </summary>
        AllowQtyBelow0 = 1,

        /// <summary>
        /// Allow qty below 0 and notify customer
        /// </summary>
        AllowQtyBelow0AndNotifyCustomer = 2,
    }
}
