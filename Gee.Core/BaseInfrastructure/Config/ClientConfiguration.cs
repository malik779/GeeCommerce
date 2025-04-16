using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure.Config
{
    public class Client
    {
        public string? ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? BaseUrl { get; set; }
        public string? TokenTime { get; set; }
        public string? PostLogoutRedirectUris { get; set; }
        public string? RedirectUrl { get; set; }
        public List<string>? AllowedScopes { get; set; } = new List<string>();
        public List<string>? AllowedGrantTypes { get; set; } = new List<string>();
        public List<string>? ApiScopes { get; set; } = new List<string>();
        public List<string>? ApiResources { get; set; } = new List<string>();
    }
    public class BaseClientResources
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public List<string>? Scopes { get; set;} = new List<string>();
    }
}
