using Gee.Core.Domain;
using Gee.Core;
    public class TenantDetailModel : BaseEntityModel<int, int, int>, ISoftDeletedEntity
    {
        public string? PhoneNumber { get; set; }
        public string? PrivacyEmail { get; set; }
        public string? ContactEmail { get; set; }
        public string? ExternalSiteUrl { get; set; }
        public string? ExternalApiJsonConfig { get; set; }
        public bool Deleted { get; set;}
    }
