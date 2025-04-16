﻿using Gee.Core.MultiTenancy.Infrastructure.DependencyInjection;
using Gee.Core.MultiTenancy.Infrastructure.StrategyAccessor;
using Gee.Core.MultiTenancy.Infrastructure;
using Gee.Core.MultiTenancy.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gee.Core.MultiTenancy.Registeration
{
   
        public class TenantBuilder<T>(IServiceCollection Services, MultiTenantOptions<T> options) where T : ITenantInfo
        {
            /// <summary>
            /// Register the tenant resolver implementation
            /// </summary>
            /// <typeparam name="V"></typeparam>
            /// <param name="lifetime"></param>
            /// <returns></returns>
            public TenantBuilder<T> WithResolutionStrategy<V>() where V : class, ITenantResolutionStrategy
            {
                Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                Services.TryAddSingleton(typeof(ITenantResolutionStrategy), typeof(V));
                return this;
            }

            /// <summary>
            /// Helper for host resolution strategy
            /// </summary>
            /// <returns></returns>
            public TenantBuilder<T> WithHostResolutionStrategy()
            {
                return WithResolutionStrategy<HostResolutionStrategy>();
            }

            /// <summary>
            /// Register the tenant lookup service implementation
            /// </summary>
            /// <typeparam name="V"></typeparam>
            /// <param name="lifetime"></param>
            /// <returns></returns>
            public TenantBuilder<T> WithTenantLookupService<V>() where V : class, ITenantLookupService<T>
            {
                Services.TryAddSingleton<ITenantLookupService<T>, V>();
                return this;
            }

            /// <summary>
            /// Register the tenant lookup service implementation
            /// </summary>
            /// <typeparam name="V"></typeparam>
            /// <param name="lifetime"></param>
            /// <returns></returns>
            public TenantBuilder<T> WithInMemoryTenantLookupService(IEnumerable<T> tenants)
            {
                var service = new InMemoryLookupService<T>(tenants);
                Services.TryAddSingleton<ITenantLookupService<T>>(service);
                return this;
            }


            /// <summary>
            /// Register tenant specific services
            /// </summary>
            /// <param name="configuration"></param>
            /// <returns></returns>
            public TenantBuilder<T> WithTenantedServices(Action<IServiceCollection, T?> configuration)
            {
                //Replace the default service provider with a multitenant service provider
                if (!options.DisableAutomaticPipelineRegistration)
                    Services.Insert(0, ServiceDescriptor.Transient<IStartupFilter>(provider => new MultitenantRequestServicesStartupFilter<T>()));

                //Register the multi-tenant service provider
                Services.AddSingleton<IMultiTenantServiceProviderFactory, MultiTenantServiceScopeFactory<T>>();
                Services.AddSingleton(new MultiTenantServiceProviderFactory<T>(Services, configuration));

                return this;
            }

            /// <summary>
            /// Configure tenant specific options
            /// </summary>
            /// <typeparam name="TOptions"></typeparam>
            /// <param name="tenantOptionsConfiguration"></param>
            /// <returns></returns>
            public TenantBuilder<T> WithTenantedConfigure<TOptions>(Action<TOptions, T?> tenantOptionsConfiguration) where TOptions : class
            {
                Services.AddOptions();

                Services.TryAddSingleton<IOptionsMonitorCache<TOptions>, MultiTenantOptionsCache<TOptions, T>>();
                Services.TryAddScoped<IOptionsSnapshot<TOptions>>((sp) =>
                {
                    return new MultiTenantOptionsManager<TOptions>(sp.GetRequiredService<IOptionsFactory<TOptions>>(), sp.GetRequiredService<IOptionsMonitorCache<TOptions>>());
                });
                Services.TryAddSingleton<IOptions<TOptions>>((sp) =>
                {
                    return new MultiTenantOptionsManager<TOptions>(sp.GetRequiredService<IOptionsFactory<TOptions>>(), sp.GetRequiredService<IOptionsMonitorCache<TOptions>>());
                });

                Services.AddSingleton<IConfigureOptions<TOptions>, ConfigureOptions<TOptions>>((IServiceProvider sp) =>
                {
                    var tenantAccessor = sp.GetRequiredService<IMultiTenantContextAccessor<T>>();
                    return new ConfigureOptions<TOptions>((options) => tenantOptionsConfiguration(options, tenantAccessor.TenantInfo));

                });

                return this;
            }
        }
}
