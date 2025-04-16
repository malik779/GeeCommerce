using Gee.Core;

public class ProductMapping : BaseEntity<int, int, int>
{
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the category identifier
    /// </summary>
    /// <summary>
    /// Gets or sets a value indicating whether the product is featured
    /// </summary>
    public bool IsFeaturedProduct { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }

    public Product? Product  { get; set; }
}
    