using Gee.Core.BaseInfrastructure.DataProviders;
using Gee.Core.CommonService;
using Gee.Core.Enum;
using LinqToDB.DataProvider.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure.Config
{
    public partial class DataProviderManager<TContext> : IDataProviderManager<TContext> where TContext : DbContext
    {
        #region Methods

        /// <summary>
        /// Gets data provider by specific type
        /// </summary>
        /// <param name="dataProviderType">Data provider type</param>
        /// <returns></returns>
        public static IBaseDataProvider GetDataProvider(DataProviderType dataProviderType,TContext dbContext)
        {
            return dataProviderType switch
            {
                DataProviderType.SqlServer => new MSSqlDataProvider<TContext>(dbContext),
                //DataProviderType.MySql => new MySqlNopDataProvider(),
                //DataProviderType.PostgreSQL => new PostgreSqlDataProvider(),
                _ => throw new Exception($"Not supported data provider name: '{dataProviderType}'"),
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        public IBaseDataProvider GetDataProvider(TContext dbContext)
        {
            

                
                var dataProviderType = Singleton<DataConfig>.Instance.DataProvider;

                return GetDataProvider(dataProviderType, dbContext);
            
        }

        #endregion
    }
}
