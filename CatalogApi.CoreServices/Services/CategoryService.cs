
using Gee.Core.BaseInfrastructure;
using Gee.Core.Responses;
using System.Linq.Expressions;
using CatalogApi.CoreServices.ModelFactories;
using Gee.Core.Extensions;
using System.Net;
using CatalogApi.Domain.Models.Category;
using CatalogApi.CoreServices.Services.Interfaces;
namespace CatalogApi.CoreServices.Services
{
    public class CategoryService : ICategoryService
    {
        private IBaseRepository<Category> _repository;
        private CategoryModelFactory _modelPrepareFactory;

        public CategoryService(IBaseRepository<Category> repository, CategoryModelFactory modelPrepareFactory)
        {
            _repository=repository;
           _modelPrepareFactory=modelPrepareFactory;
        }
        public async Task<Response<CategoryModel>> CreateAsync(CategoryModel request)
        {
            var category = request.ToEntity<Category>();
            category.CreatedOnUtc = DateTime.UtcNow;
            category.CreatedBy = 1037;
            await _repository.InsertAsync(category);
            _repository.Save();

            var response = new Response<CategoryModel>(category.ToModel<CategoryModel>(), System.Net.HttpStatusCode.OK, "success!");
            return response;
        }

        public Task<Response<CategoryModel>> DeleteAsync(CategoryModel request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<CategoryModel>> FindByIdAsync(CategoryModel request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<CategoryModel>> FindByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedFilterModel<CategoryModel>> GetAllAsync(CategoryModel request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<CategoryModel>> GetByAsync(Expression<Func<CategoryModel, bool>> Expression)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<CategoryModel>> GetById(int Id)
        {
            
            var category=await _repository.GetByIdAsync(Id);
           
            if(category == null)
            {
                return new Response<CategoryModel>(null,HttpStatusCode.OK,"No record found!");
            }

            var model = await _modelPrepareFactory.PrepareWithDataModelAsync(new CategoryModel(), category);
            return new Response<CategoryModel>(model);
        }

        public Task<Response<CategoryModel>> UpdateAsync(CategoryModel request)
        {
            throw new NotImplementedException();
        }
    }
}
