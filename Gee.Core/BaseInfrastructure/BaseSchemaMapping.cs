using Gee.Core.BaseInfrastructure.Config;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure
{
    public static class BaseSchemaMapping
    {
        public static MappingSchema GetMappingSchema(string configurationName, IDataProvider mappings)
        {

            if (Singleton<MappingSchema>.Instance is null)
            {
                Singleton<MappingSchema>.Instance = new MappingSchema(configurationName, mappings.MappingSchema);
               // Singleton<MappingSchema>.Instance.AddMetadataReader(new FluentMigratorMetadataReader());
            }

            return Singleton<MappingSchema>.Instance;
        }
    }
}
