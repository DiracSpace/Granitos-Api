using Microsoft.Extensions.Hosting;

namespace Granitos.Common.Extensions;

internal static class LocalEnvironments
{
    public static readonly string Localhost = nameof(Localhost);
}

public static class HostingEnvironmentExtensions
{
    public static bool IsLocalhost(this IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment, nameof(hostEnvironment));
        return hostEnvironment.IsEnvironment(LocalEnvironments.Localhost);
    }
}