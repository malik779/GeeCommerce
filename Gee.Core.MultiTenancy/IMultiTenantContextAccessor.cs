namespace Gee.Core.MultiTenancy
{    /// <summary>
     /// Provides access to the current tenant context
     /// </summary>
    public interface IMultiTenantContextAccessor<T> where T : ITenantInfo
    {
        /// <summary>
        /// Current tenant
        /// </summary>
        T? TenantInfo { get; set; }
    }
}