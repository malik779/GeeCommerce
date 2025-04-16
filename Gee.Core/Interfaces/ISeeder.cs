using Gee.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.Interfaces
{
    public interface ISeeder<TContext>:IDbSeeder<TContext> where TContext:DbContext
    {
    }
}
