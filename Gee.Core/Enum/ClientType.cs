using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.Enum
{
    public enum ClientType
    {
        [EnumMember(Value = "FrontClient")]
        FrontClient,
        [EnumMember(Value = "ApiClient")]
        ApiClient
}
}
