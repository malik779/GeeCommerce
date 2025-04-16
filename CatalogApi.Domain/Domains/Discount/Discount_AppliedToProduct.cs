using CatalogApi.Service.InfraStructure.HelperClasses;

public class Discount_AppliedToProduct: AppliedDiscountMapping
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
