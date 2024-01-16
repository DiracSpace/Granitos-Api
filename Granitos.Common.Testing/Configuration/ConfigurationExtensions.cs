using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Testing.Configuration;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder configuration, string environmentName,
        bool optional = true,
        bool reloadOnChange = true)
    {
        return configuration
            .AddJsonFile($"appsettings.json",
                optional: optional,
                reloadOnChange: reloadOnChange)
            .AddJsonFile($"appsettings.{environmentName}.json",
                optional: optional,
                reloadOnChange: reloadOnChange)
            .AddEnvironmentVariables();
    }

    public static IServiceCollection AddConfigurationStub(this IServiceCollection services, Dictionary<string, string?>? appSettingsStub)
    {
        var configurationStub = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

        return services
            .AddSingleton<IConfiguration>(configurationStub);
    }

    public static IServiceCollection AddConfigurationStub(this IServiceCollection services, string environmentName,
        bool optional = true,
        bool reloadOnChange = true)
    {
        var configurationStub = new ConfigurationBuilder()
            .AddAppSettings(
                environmentName,
                optional: optional,
                reloadOnChange: reloadOnChange)
            .Build();

        return services
            .AddSingleton<IConfiguration>(configurationStub);
    }
}