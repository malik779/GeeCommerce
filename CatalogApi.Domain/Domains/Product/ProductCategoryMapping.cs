
public partial class ProductCategoryMapping : ProductMapping
{

    /// <summary>
    /// Gets or sets the category identifier
    /// </summary>
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}