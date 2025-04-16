using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Cryptography.X509Certificates;
namespace Gee.DefaultServices
{
    public static class AuthenticationExtentions
    {/// <summary>
    /// Default Authentication System for APIs
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
        public static IServiceCollection AddDefaultAuthentication(this IHostApplicationBuilder builder,string? certPath,string? certPassword)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            var identitySection = configuration.GetSection("Identity");

            if (!identitySection.Exists())
            {
                // No identity section, so no authentication
                return services;
            }

            // prevent from mapping "sub" claim to nameidentifier.
            JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
            var identityUrl = identitySection.GetRequiredSection("IdentityUrl");
            var cert = new X509Certificate2(certPath??"", certPassword);
            var key = new X509SecurityKey(cert);

            services.AddAuthentication()
             .AddJwtBearer(options =>//configuration for simple jwt token 
             {
                 HttpClientHandler handler = new HttpClientHandler();
                 var audience = identitySection.GetRequiredSection("Audience");
                 options.Authority = identityUrl?.Value;
                 options.Audience = "catalogApi";
                 options.RequireHttpsMetadata = false;
                 
                 options.TokenValidationParameters = new TokenValidationParameters
                 { 
                     // The signing key must match!
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = key,
                     ValidateIssuer = true,
                     ValidateAudience = false,
                     ValidIssuer = identityUrl?.Value,  // Matches token's `iss` claim
                      
                 };
                 handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                 {
                     return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
                 };
                 


               //  options.BackchannelHttpHandler = handler;

             });
            services.AddAuthorization();

            return services;
        }
    }
}
