using Gee.Core;
using Gee.Core.Domain;

namespace TenantApi.Service.Infrastructure.Domain
{
    public class Tenant : BaseEntity<int, int, int>, ISoftDeletedEntity
    {
        public required string Name { get; set; }
        public string? Domain { get; set; }
        public string? CertificatePath { get; set; }
        public string? CertificatePassword { get; set; }
        public string? Type { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set ; }
        public TenantDetail? TenantDetail { get; set; }  
        public ThemeDetail? ThemeDetail { get; set; }
    }
}
