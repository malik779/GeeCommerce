using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.MultiTenancy.Services
{
    internal class InMemoryLookupService<T>(IEnumerable<T> Tenants) : ITenantLookupService<T> where T : ITenantInfo
    {
        public Task<T?> GetTenantAsync(string identifier)
        {
            return Task.FromResult(Tenants.SingleOrDefault(t => t.Identifier == identifier));
        }
    }
}
