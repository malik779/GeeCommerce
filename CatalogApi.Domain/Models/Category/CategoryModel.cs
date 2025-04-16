using Gee.Core;
using Gee.Core.BaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CatalogApi.Domain.Models.Category
{
    public class CategoryModel: BaseEntityModel<int,int,int>,IBaseModel
    {
        #region Ctor

        public CategoryModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

           // Locales = new List<CategoryLocalizedModel>();
            AvailableCategoryTemplates = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            AvailableDiscounts = new List<SelectListItem>();
            SelectedDiscountIds = new List<int>();

            SelectedCustomerRoleIds = new List<int>();
            AvailableCustomerRoles = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();

            CategoryProductSearchModel = new BaseSearchModel();
        }

        #endregion

        #region Properties

        public string Name { get; set; }


        public string Description { get; set; }


        public int CategoryTemplateId { get; set; }
        public IList<SelectListItem> AvailableCategoryTemplates { get; set; }


        public string MetaKeywords { get; set; }


        public string MetaDescription { get; set; }


        public string MetaTitle { get; set; }


        public string SeName { get; set; }


        public int ParentCategoryId { get; set; }


        public int PictureId { get; set; }


        public int PageSize { get; set; }


        public bool AllowCustomersToSelectPageSize { get; set; }


        public string PageSizeOptions { get; set; }


        public bool PriceRangeFiltering { get; set; }


        public decimal PriceFrom { get; set; }


        public decimal PriceTo { get; set; }


        public bool ManuallyPriceRange { get; set; }


        public bool ShowOnHomepage { get; set; }


        public bool IncludeInTopMenu { get; set; }

        public bool Published { get; set; }


        public bool Deleted { get; set; }


        public int DisplayOrder { get; set; }

        //public IList<CategoryLocalizedModel> Locales { get; set; }

        public string Breadcrumb { get; set; }


        public IList<int> SelectedCustomerRoleIds { get; set; }
        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        //store mapping

        public IList<int> SelectedStoreIds { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        //discounts

        public IList<int> SelectedDiscountIds { get; set; }
        public IList<SelectListItem> AvailableDiscounts { get; set; }

        public BaseSearchModel CategoryProductSearchModel { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }

        #endregion
    }
}
