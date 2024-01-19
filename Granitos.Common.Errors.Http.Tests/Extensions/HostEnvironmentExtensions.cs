using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Granitos.Common.Errors.Http.Tests.Extensions;

internal static class HostEnvironmentExtensions
{
    public static IServiceCollection AddHostEnvironmentStub(this IServiceCollection services)
    {
        return services.AddSingleton<IHostEnvironment, HostEnvironmentStub>();
    }

    internal class HostEnvironmentStub : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = "localhost";
        public string ApplicationName { get; set; } = default!;
        public string ContentRootPath { get; set; } = default!;
        public IFileProvider ContentRootFileProvider { get; set; } = default!;
    }
}
