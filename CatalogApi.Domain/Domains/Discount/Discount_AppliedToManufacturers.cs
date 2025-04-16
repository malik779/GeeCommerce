using CatalogApi.Service.InfraStructure.HelperClasses;

public class Discount_AppliedToManufacturers: AppliedDiscountMapping
    {
        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }
