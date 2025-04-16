using Gee.Core.MultiTenancy;

namespace Gee.Core.BaseModels
{
    public class TenantBaseModel: TenantModel,ITenantInfo
    {
        public new int? Id { get; set; }
        public string? Identifier { get; set; }
    }
}
