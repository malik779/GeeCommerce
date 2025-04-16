using Gee.Core.BaseInfrastructure.Config;
using Gee.Core.BaseInfrastructure.DataProviders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.CommonService
{
    public static class ServiceLocator
    {
        private static IServiceProvider? _serviceProvider;

        public static IServiceProvider SetServiceProvider(this IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return _serviceProvider;
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
