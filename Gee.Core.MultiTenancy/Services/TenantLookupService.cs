using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.MultiTenancy.Services
{
    public class TenantLookupService<T> : ITenantLookupService<T> where T : class,ITenantInfo
    {
        public TenantLookupService() 
        {
        
        }
        public virtual Task<T?> GetTenantAsync(string domain)
        {
            throw new NotImplementedException();
        }
    }
}
