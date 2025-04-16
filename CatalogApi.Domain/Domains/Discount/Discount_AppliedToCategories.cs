
using CatalogApi.Service.InfraStructure.HelperClasses;

public partial class Discount_AppliedToCategories: AppliedDiscountMapping
{
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }

