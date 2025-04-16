using AutoMapper;
using CatalogApi.Domain.Models.Category;
using Gee.Core.Interfaces;

namespace CatalogApi.CoreServices.MapperConfigurations
{
    public class AdminMapperConfiguration : Profile, IOrderedMapperProfile
    {
        public AdminMapperConfiguration()
        {
            CreateCategoryMaps();
        }

        protected virtual void CreateCategoryMaps()
        {
            CreateMap<Category, CategoryModel>()
               .ForMember(model => model.AvailableCategories, options => options.Ignore())
               .ForMember(model => model.AvailableCategoryTemplates, options => options.Ignore())
               .ForMember(model => model.Breadcrumb, options => options.Ignore())
               .ForMember(model => model.CategoryProductSearchModel, options => options.Ignore())
               .ForMember(model => model.SeName, options => options.Ignore())
               .ForMember(model => model.PrimaryStoreCurrencyCode, options => options.Ignore());
            CreateMap<CategoryModel, Category>()
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.Deleted, options => options.Ignore())
                .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore());

            //CreateMap<CategoryTemplate, CategoryTemplateModel>();
            //CreateMap<CategoryTemplateModel, CategoryTemplate>();
        }
        public int Order => 1;
    }
}
