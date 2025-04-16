using Gee.Core;

namespace TenantApi.Service.Infrastructure.Domain
{
    public class TenantDetail:TenantBaseEntity<int,int,int>
    {
        public string? PhoneNumber { get;set; }
        public string? PrivacyEmail { get;set; }
        public string? ContactEmail { get;set; }
        public string? ExternalSiteUrl { get;set; }
        public string? ExternalApiJsonConfig { get;set; }
        public virtual Tenant? Tenant { get; set; }
    }
}
