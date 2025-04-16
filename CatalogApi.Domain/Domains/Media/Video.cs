using Gee.Core;

/// <summary>
/// Represents a video
/// </summary>
public partial class Video : TenantBaseEntity<int, int, int>
{
    /// <summary>
    /// Gets or sets the URL of video
    /// </summary>
    public string VideoUrl { get; set; }
}