using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.MultiTenancy.Services
{
    public interface ITenantLookupService<T> where T: ITenantInfo
    {
        /// <summary>
        /// Given an identifier, it returns the durable tenant id
        /// </summary>
        /// <returns></returns>
        Task<T?> GetTenantAsync(string domain);
    }
}
