using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.MultiTenancy.Infrastructure.StrategyAccessor
{
    public interface ITenantResolutionStrategy
    {
        /// <summary>
        /// Get the current tenant identifier
        /// </summary>
        /// <returns></returns>
        Task<string?> GetTenantIdentifierAsync();
    }
}
