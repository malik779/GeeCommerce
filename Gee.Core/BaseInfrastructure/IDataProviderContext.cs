using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure
{
    public interface IDataProviderContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity<int, int, int>;
        // Add other necessary methods or properties
    }
}
