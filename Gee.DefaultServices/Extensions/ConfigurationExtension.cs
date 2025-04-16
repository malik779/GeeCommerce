using Microsoft.Extensions.Configuration;

namespace Gee.DefaultServices.Extensions
{
    public static class ConfigurationExtension
    {
        public static string GetRequiredValue(this IConfiguration configuration, string name) =>
            configuration[name] ?? throw new InvalidOperationException($"Configuration missing value for: {(configuration is IConfigurationSection s ? s.Path + ":" + name : name)}");
    }
}
