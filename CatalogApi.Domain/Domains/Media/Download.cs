using Gee.Core;
/// <summary>
/// Represents a download
/// </summary>
public partial class Download : TenantBaseEntity<int,int,int>
{
    /// <summary>
    /// Gets or sets a GUID
    /// </summary>
    public Guid DownloadGuid { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether DownloadUrl property should be used
    /// </summary>
    public bool UseDownloadUrl { get; set; }

    /// <summary>
    /// Gets or sets a download URL
    /// </summary>
    public string? DownloadUrl { get; set; }

    /// <summary>
    /// Gets or sets the download binary
    /// </summary>
    public byte[] DownloadBinary { get; set; }

    /// <summary>
    /// The mime-type of the download
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// The filename of the download
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Gets or sets the extension
    /// </summary>
    public string? Extension { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the download is new
    /// </summary>
    public bool IsNew { get; set; }
}