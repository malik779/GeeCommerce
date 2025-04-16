using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseModelFactories
{
    public partial interface IBasePrepareModelFactory<TModel, TSearchModel, TRelationalModel> where TRelationalModel : class?
    {
        /// <summary>
        /// Prepare category search model
        /// </summary>
        /// <param name="searchModel">Category search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the category search model
        /// </returns>
        Task<TSearchModel> PrepareSearchModelAsync(TSearchModel searchModel);

        /// <summary>
        /// Prepare paged category list model
        /// </summary>
        /// <param name="searchModel">Category search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the category list model
        /// </returns>
        Task<List<TModel>> PrepareListModelAsync(TSearchModel searchModel);

        /// <summary>
        /// Prepare category model
        /// </summary>
        /// <param name="model">Category model</param>
        /// <param name="category">Category</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the category model
        /// </returns>
        Task<TModel> PrepareWithDataModelAsync(TModel model, object Data, bool excludeProperties = false);

        /// <summary>
        /// Prepare paged category product list model
        /// </summary>
        /// <param name="searchModel">Category product search model</param>
        /// <param name="category">Category</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the category product list model
        /// </returns>
        Task<TRelationalModel> PrepareRelationalItemListModelAsync(TSearchModel searchModel, object Data);

        /// <summary>
        /// Prepare product search model to add to the category
        /// </summary>
        /// <param name="searchModel">Product search model to add to the category</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product search model to add to the category
        /// </returns>
        ///Task<TModel> PrepareAddItemToMappedItemSearchModelAsync<TRequest, TModel>(TRequest searchModel);

        /// <summary>
        /// Prepare paged product list model to add to the category
        /// </summary>
        /// <param name="searchModel">Product search model to add to the category</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product list model to add to the category
        /// </returns>
        //Task<TModel> PrepareAddItemToMappedItemListModelAsync<TRequest, TModel>(TRequest searchModel);
    }
}
