namespace CatalogApi.Service.InfraStructure.HelperClasses
{
    public enum DiscountLimitationTypeEnum
    {
        /// <summary>
        /// None
        /// </summary>
        Unlimited = 0,

        /// <summary>
        /// N Times Only
        /// </summary>
        NTimesOnly = 15,

        /// <summary>
        /// N Times Per Customer
        /// </summary>
        NTimesPerCustomer = 25
    }
}