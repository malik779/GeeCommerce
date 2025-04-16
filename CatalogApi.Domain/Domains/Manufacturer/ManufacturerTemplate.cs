using Gee.Core;

public partial class ManufacturerTemplate : BaseEntity<int, int, int>
{
    /// <summary>
    /// Gets or sets the template name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the view path
    /// </summary>
    public string ViewPath { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}
