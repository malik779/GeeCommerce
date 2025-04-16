using Gee.Core;

public partial class ProductManufacturerMapping : ProductMapping
{

    /// <summary>
    /// Gets or sets the category identifier
    /// </summary>
    public int ManufacturerId { get; set; }
    public Manufacturer? Manufacturer { get; set; }
}