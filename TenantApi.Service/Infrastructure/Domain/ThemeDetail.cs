using Gee.Core;
using Gee.Core.Domain;

namespace TenantApi.Service.Infrastructure.Domain
{
    public class ThemeDetail : TenantBaseEntity<int, int, int>, ISoftDeletedEntity
    {
        public string? ThemeJson { get; set; }
        public bool Deleted { get ; set; }
        public virtual Tenant? Tenant { get; set; }
    }
}
