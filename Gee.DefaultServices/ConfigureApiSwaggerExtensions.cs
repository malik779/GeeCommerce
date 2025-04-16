using Asp.Versioning;
using Gee.DefaultServices.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;
namespace Gee.DefaultServices
{
    public static class ConfigureApiSwaggerExtensions
    {
        public static IApplicationBuilder UseDefaultSwagger(this WebApplication builder)
        {
            var configuration = builder.Configuration;
            var SwaggerOption = configuration.GetSection("SwaggerOptions");
            var openApiSection = SwaggerOption?.GetSection("OpenApi");

            if (!openApiSection.Exists())
            {
                return builder;
            }

            builder.UseSwagger();

            if (builder.Environment.IsDevelopment())
            {
                builder.UseSwaggerUI(setup =>
                {


                    var pathBase = configuration["PATH_BASE"] ?? string.Empty;

                    var authSection = SwaggerOption?.GetSection("Auth");
                    var endpointSection = openApiSection.GetRequiredSection("Endpoint");

                    foreach (var description in builder.DescribeApiVersions())
                    {
                        var name = description.GroupName;
                        var url = endpointSection["Url"] ?? $"{pathBase}/swagger/{name}/swagger.json";

                        setup.SwaggerEndpoint(url, name);
                    }

                    if (authSection.Exists())
                    {
                        setup.OAuthClientId(authSection.GetRequiredValue("ClientId"));
                        setup.OAuthAppName(authSection.GetRequiredValue("AppName"));
                    }
                });

                // Add a redirect from the root of the app to the swagger endpoint
                builder.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

            }
            return builder;
        }
        public static IHostApplicationBuilder AddDefaultSwaggerGen(
            this IHostApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;
            var apiVersioning=services.AddApiVersioning();
            var SwaggerOption = configuration.GetSection("SwaggerOptions");
            var openApi = SwaggerOption.GetSection("OpenApi");

            if (!openApi.Exists())
            {
                return builder;
            }

            services.AddEndpointsApiExplorer();

            if (apiVersioning is not null)
            {
                // the default format will just be ApiVersion.ToString(); for example, 1.0.
                // this will format the version as "'v'major[.minor][-status]"
                apiVersioning.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");
                services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
                services.AddSwaggerGen(options => options.OperationFilter<OpenApiDefaultValues>());
            }

            return builder;
        }
    }

    internal sealed class OpenApiDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            // remove any assumed media types not present in the api description
            foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
            {
                var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
                var response = operation.Responses[responseKey];

                foreach (var contentType in response.Content.Keys)
                {
                    if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                    {
                        response.Content.Remove(contentType);
                    }
                }
            }

            if (operation.Parameters == null)
            {
                return;
            }

            // fix-up parameters with additional information from the api explorer that might
            // not have otherwise been used. this will most often happen for api version parameters
            // which are dynamically added, have no endpoint signature info, nor any xml comments.
            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                parameter.Description ??= description.ModelMetadata?.Description;

                if (parameter.Schema.Default == null &&
                    description.DefaultValue != null &&
                    description.DefaultValue is not DBNull &&
                    description.ModelMetadata is ModelMetadata modelMetadata)
                {
                    var json = JsonSerializer.Serialize(description.DefaultValue, modelMetadata.ModelType);
                    parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}
