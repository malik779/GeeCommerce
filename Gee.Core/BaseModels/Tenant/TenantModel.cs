using Gee.Core;
using Gee.Core.Domain;

public class TenantModel : BaseEntityModel<int, int, int>, ISoftDeletedEntity, IBaseModel
{


    #region Properties
    public string? Name { get; set; }
    public string? Domain { get; set; }
    public string? CertificatePath { get; set; }
    public string? CertificatePassword { get; set; }
    public string? Type { get; set; }
    public bool IsActive { get; set; }
    public bool Deleted { get; set; }
    public TenantDetailModel? TenantDetail { get; set; }
    public ThemeDetailModel? ThemeDetail { get; set; }
    public string? Description { get; set; }

    public string? MetaKeywords { get; set; }


    public string? MetaDescription { get; set; }


    public string? MetaTitle { get; set; }


    public string? SeName { get; set; }
    public int PageSize { get; set; }


    public bool AllowCustomersToSelectPageSize { get; set; }


    public string? PageSizeOptions { get; set; }
    public bool PriceRangeFiltering { get; set; }


    public decimal PriceFrom { get; set; }


    public decimal PriceTo { get; set; }
    public bool ShowOnHomepage { get; set; }


    public bool IncludeInTopMenu { get; set; }

    public bool Published { get; set; }

    public bool ManuallyPriceRange { get; set; }
    #endregion
}
