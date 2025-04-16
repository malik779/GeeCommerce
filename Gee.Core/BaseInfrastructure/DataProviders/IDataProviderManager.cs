using Microsoft.EntityFrameworkCore;

namespace Gee.Core.BaseInfrastructure.DataProviders
{
    public partial interface IDataProviderManager<TContext> where TContext : DbContext
    {
        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        IBaseDataProvider GetDataProvider(TContext context);

        #endregion
    }
}