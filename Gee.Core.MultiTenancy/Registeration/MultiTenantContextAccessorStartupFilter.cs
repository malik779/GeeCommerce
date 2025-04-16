using Gee.Core.MultiTenancy.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.MultiTenancy.Registeration
{
    internal class MultiTenantContextAccessorStartupFilter<T>() : IStartupFilter where T : ITenantInfo
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<MultiTenantContextAccessorMiddleware<T>>();
                next(builder);
            };
        }
    }
}
