using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Concurrent;

namespace Gee.Core.MultiTenancy.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Root Service Provider IServiceProvider which decide the root startup container for tenant specific
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="containerBuilder"></param>
    /// <param name="tenantServiceConfiguration"></param>
    public class MultiTenantServiceProviderFactory<T>(IServiceCollection containerBuilder, Action<IServiceCollection, T?> tenantServiceConfiguration)  where T: ITenantInfo
    {

        private readonly ConcurrentDictionary<string, Lazy<IServiceProvider>> _compiledProviders = new();

        //Cache compiled providers
        private readonly ConcurrentDictionary<string, Lazy<IServiceProvider>> CompiledProviders = new();
        public IServiceProvider GetServiceProviderForTenant(T tenant)
        {
            var identifier = tenant?.Id?.ToString() ?? tenant?.Identifier ?? "default";

            return _compiledProviders.GetOrAdd(identifier, key => new Lazy<IServiceProvider>(() =>
            {
                var container = new ServiceCollection();

                // Add default services
                foreach (var service in containerBuilder)
                {
                    container.Add(service);
                }

                // Add tenant-specific services
                tenantServiceConfiguration(container, tenant);

                return container.BuildServiceProvider();
            })).Value;
        }
    }
    /// <summary>
    /// Factory wrapper for creating service scopes tell which service or middlewares are for which tenant 
    /// </summary>
    /// <param name="serviceProvider"></param>
    internal class MultiTenantServiceScopeFactory<T>(MultiTenantServiceProviderFactory<T> ServiceProviderFactory, IMultiTenantContextAccessor<T> multiTenantContextAccessor) : IMultiTenantServiceProviderFactory where T : ITenantInfo
    {

        /// <summary>
        /// Create scope
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateScope()
        {
            var tenant = multiTenantContextAccessor.TenantInfo ?? throw new InvalidOperationException("Tenant context is not available");
            return ServiceProviderFactory.GetServiceProviderForTenant(tenant).CreateScope();
        }
    }
}
