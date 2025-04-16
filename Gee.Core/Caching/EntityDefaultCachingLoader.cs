using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.Caching
{
    public static class EntityDefaultCachingLoader<TEntity> where TEntity : BaseEntity<int,int,int>
    {
            /// <summary>
            /// Gets an entity type name used in cache keys
            /// </summary>
            public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

            /// <summary>
            /// Gets a key for caching entity by identifier
            /// </summary>
            /// <remarks>
            /// {0} : entity id
            /// </remarks>
            public static CacheKey ByIdCacheKey => new($"Gee.{EntityTypeName}.byid.{{0}}", ByIdPrefix, Prefix);

            /// <summary>
            /// Gets a key for caching entities by identifiers
            /// </summary>
            /// <remarks>
            /// {0} : entity ids
            /// </remarks>
            public static CacheKey ByIdsCacheKey => new($"Gee.{EntityTypeName}.byids.{{0}}", ByIdsPrefix, Prefix);

            /// <summary>
            /// Gets a key for caching all entities
            /// </summary>
            public static CacheKey AllCacheKey => new($"Gee.{EntityTypeName}.all.", AllPrefix, Prefix);

            /// <summary>
            /// Gets a key pattern to clear cache
            /// </summary>
            public static string Prefix => $"Gee.{EntityTypeName}.";

            /// <summary>
            /// Gets a key pattern to clear cache
            /// </summary>
            public static string ByIdPrefix => $"Gee.{EntityTypeName}.byid.";

            /// <summary>
            /// Gets a key pattern to clear cache
            /// </summary>
            public static string ByIdsPrefix => $"Gee.{EntityTypeName}.byids.";

            /// <summary>
            /// Gets a key pattern to clear cache
            /// </summary>
            public static string AllPrefix => $"Gee.{EntityTypeName}.all.";
        }
}
