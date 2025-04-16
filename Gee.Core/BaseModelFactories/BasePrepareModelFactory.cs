using Gee.Core.Extensions;
namespace Gee.Core.BaseModelFactories
{
    public abstract class BasePrepareModelFactory<TModel, TSearchModel, TRelationalModel> : IBasePrepareModelFactory<TModel, TSearchModel, TRelationalModel>
        where TRelationalModel : class?
        where TModel : BaseEntityModel<int, int, int>, IBaseModel
    {
        public  virtual Task<List<TModel>> PrepareListModelAsync(TSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TRelationalModel> PrepareRelationalItemListModelAsync(TSearchModel searchModel, dynamic Data)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TSearchModel> PrepareSearchModelAsync(TSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public async Task<TModel> PrepareWithDataModelAsync(TModel model, dynamic Data, bool excludeProperties = false)
        {
            //Func<CategoryLocalizedModel, int, Task> localizedModelConfiguration = null;

            if (Data != null)
            {
                //fill in model values from the entity
               
                if (Data is BaseEntity<int, int, int> entityData)
                {
                    model=entityData.ToModel<TModel>();
                }
             }

            //set default values for the new model
            if (Data == null && !excludeProperties)
            {
                //model.PageSize = _catalogSettings.DefaultCategoryPageSize;
                //model.PageSizeOptions = _catalogSettings.DefaultCategoryPageSizeOptions;
                model.Published = true;
                model.IncludeInTopMenu = true;
                model.AllowCustomersToSelectPageSize = true;
                model.PriceRangeFiltering = true;
                model.ManuallyPriceRange = true;
                model.PriceFrom = 0;
                model.PriceTo = 10000;
            }

            //model.PrimaryStoreCurrencyCode = (await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimaryStoreCurrencyId)).CurrencyCode;
            return await Task.Run(() =>
            {
                return model;
            });
          

            
        }
        
    }
}
