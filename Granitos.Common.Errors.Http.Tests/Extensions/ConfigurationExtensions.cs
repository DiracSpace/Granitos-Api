using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Errors.Http.Tests.Extensions;

internal static class ConfigurationExtensions
{
    private static readonly Dictionary<string, string?> _appSettingsStub = new()
    {
        //["Errors:DocsUrl"] = "uri://edgraph.com",
    };

    public static IServiceCollection AddConfigurationStub(this IServiceCollection services)
    {
        return services.AddConfigurationStub(_appSettingsStub);
    }

    public static IServiceCollection AddConfigurationStub(this IServiceCollection services, IEnumerable<KeyValuePair<string, string?>>? appSettingsStub)
    {
        var configurationStub = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

        return services
            .AddSingleton<IConfiguration>(configurationStub);
    }

    public static IServiceCollection AddConfigurationStub(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton(configuration);
    }

    public static IConfiguration GetConfigurationStub()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(_appSettingsStub)
            .Build();
    }
}