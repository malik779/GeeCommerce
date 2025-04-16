using Gee.Core;
using Gee.Core.Domain;
public class ThemeDetailModel : BaseEntityModel<int, int, int>, ISoftDeletedEntity
{
    public string? ThemeJson { get; set; }
    public bool Deleted { get; set; }
}
