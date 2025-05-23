﻿using Gee.Core;

namespace TenantApi.Service.Infrastructure.Domain
{
    public class EmailAccountConfig:TenantBaseEntity<int,int,int>
    {
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Host { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public int MaxNumberOfEmails { get; set; }
        public int EmailAuthenticationMethodId { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
