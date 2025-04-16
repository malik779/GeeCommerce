﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.MultiTenancy.Infrastructure.StrategyAccessor
{
    internal class AsyncLocalMultiTenantContextAccessor<T>:IMultiTenantContextAccessor<T> where T : ITenantInfo
    {
        /// <summary>
        /// Provide access to current request's tenant context
        /// </summary>
        private static readonly AsyncLocal<TenantInfoHolder> asyncLocalContext = new();
        /// <summary>
        /// Get or set the current tenant context
        /// </summary>
        public T? TenantInfo
        {
            get
            {
                if (asyncLocalContext?.Value == null)
                    return default;
                return asyncLocalContext.Value.Context;
            }
            set
            {
                //Clear any trapped context as the old value is being replaced
                var holder = asyncLocalContext.Value;
                if (holder != null)
                    holder.Context = default;

                //Set the context value
                if (value != null)
                    asyncLocalContext.Value = new TenantInfoHolder { Context = value };
            }
        }

        /// <summary>
        /// Context holder to provide object indirection so that ITenantInfo
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        private class TenantInfoHolder()
        {
            public T? Context;
        }
    }
}
