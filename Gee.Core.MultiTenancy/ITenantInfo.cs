namespace Gee.Core.MultiTenancy
{
    public partial interface ITenantInfo
    {
        int? Id { get; set; }
        string? Identifier { get; set; }
    }
}