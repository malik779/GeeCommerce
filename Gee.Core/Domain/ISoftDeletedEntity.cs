using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.Domain
{
    public interface ISoftDeletedEntity
    {
        bool Deleted { get; set; }
    }
}
