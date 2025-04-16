using Microsoft.AspNetCore.Http;

namespace Gee.Core.MultiTenancy.Infrastructure.StrategyAccessor
{

    internal class HostResolutionStrategy(IHttpContextAccessor httpContextAccessor) : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<string?> GetTenantIdentifierAsync()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("HttpContext is not available");

            return await Task.FromResult(_httpContextAccessor.HttpContext.Request.Host.Host);
        }
    }
}
