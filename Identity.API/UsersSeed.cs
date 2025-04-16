
using Duende.IdentityServer.EntityFramework.DbContexts;
using Gee.Core.Interfaces;
using Duende.IdentityServer.EntityFramework.Entities;
using Gee.Core.Extensions;
using Gee.Core.BaseInfrastructure.DataProviders;
using System.Collections.Generic;
using Duende.IdentityServer.EntityFramework.Mappers;
using static System.Formats.Asn1.AsnWriter;
namespace eShop.Identity.API;

public class UsersSeed(ILogger<UsersSeed> logger, UserManager<ApplicationUser> userManager) : IDbSeeder<ApplicationDbContext>, IDbSeeder<ConfigurationDbContext>
{
    public async Task SeedAsync(ApplicationDbContext context)
    {
        var superAdmin = await userManager.FindByNameAsync("superadminsys@yopmail.com");

        if (superAdmin == null)
        {
            superAdmin = new ApplicationUser
            {
                UserName = "superadminsys@yopmail.com",
                Email = "superadminsys@yopmail.com",
                EmailConfirmed = true,
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = "1234567890",
            };

            var result = userManager.CreateAsync(superAdmin, "Abc@12345").Result;

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("super admin created");
            }
        }
        else
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("super admin already exists");
            }
        }

    }

    public async Task SeedAsync(ConfigurationDbContext context)
    {
        var DataSetting = DataSettingsManager.LoadSettings();
        var DefaultClients = DataSetting.Clients;
        if (DataSetting != null)
        {

            foreach (var client in DataSetting.Clients)
            {
                var PostLogoutUrl = client.BaseUrl;
                if (!string.IsNullOrEmpty(client?.PostLogoutRedirectUris))
                {
                    PostLogoutUrl = client.PostLogoutRedirectUris;
                }

                var RedirectUrl = client.BaseUrl;
                if (!string.IsNullOrEmpty(client?.RedirectUrl))
                {
                    RedirectUrl = client.RedirectUrl;
                }

                if (!context.Clients.Any(x => x.ClientId == client.ClientId))
                {
                    var grants = new List<ClientGrantType>();
                    grants.AddRange(client.AllowedGrantTypes.Select(data => new ClientGrantType { GrantType = data }));
                    var identityClient = new Duende.IdentityServer.EntityFramework.Entities.Client

                    {

                        ClientId = client.ClientId,
                        ClientName = client.ClientName,
                        AllowedGrantTypes = grants,
                        AllowAccessTokensViaBrowser = true,
                        RequireConsent = true,
                        ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = new Duende.IdentityServer.Models.Secret("secret".Sha256()).Value,Description="secret value will be secret" } },
                        RedirectUris = new List<ClientRedirectUri>
                    { new ClientRedirectUri {RedirectUri= RedirectUrl  }
                    },
                        PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>
                    { new ClientPostLogoutRedirectUri {PostLogoutRedirectUri= PostLogoutUrl + "/" }
                    },
                        AllowedCorsOrigins = new List<ClientCorsOrigin>
                    { new ClientCorsOrigin {Origin= client.BaseUrl }
                    },
                        AlwaysSendClientClaims = true,
                        AllowedScopes = new List<ClientScope> {
                        new ClientScope { Scope= IdentityServerConstants.StandardScopes.OpenId },
                        new ClientScope { Scope= IdentityServerConstants.StandardScopes.Profile }
                        }
                    };
                    if (client?.AllowedScopes?.Count() > 0)
                    {
                        identityClient.AllowedScopes.AddRange(client.AllowedScopes.Select(x => new ClientScope { Scope = x }));
                    }
                    await context.Clients.AddAsync(identityClient);
                }

            }


        }
        if (!context.ApiResources.Any())
        {
            foreach (var resource in DataSetting.ApiResources)
            {
                var identityResource = new Duende.IdentityServer.EntityFramework.Entities.ApiResource { Name = resource.Name, DisplayName = resource.DisplayName, Enabled = true };
                if (resource?.Scopes?.Count() > 0)
                {
                    identityResource.Scopes = new List<ApiResourceScope>();
                    foreach (var item in resource.Scopes)
                    {
                        identityResource.Scopes.Add(new ApiResourceScope { Scope = item });
                    }
                }
                await context.ApiResources.AddAsync(identityResource);

            }
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var scope in DataSetting.ApiScopes)
            {

                context.ApiScopes.Add(new Duende.IdentityServer.EntityFramework.Entities.ApiScope { Name = scope.Name, DisplayName = scope.DisplayName, Enabled = true });


            }
        }

        if (!context.IdentityResources.Any())
        {

            foreach (var resource in Config.GetResources())
            {
                var entity = resource.ToEntity();
                if (entity.Name == "profile")
                {
                    entity.UserClaims.Add(new IdentityResourceClaim { Type = "role" });
                }
                context.IdentityResources.Add(entity);

            }
        }
        context.SaveChanges();
    }

}