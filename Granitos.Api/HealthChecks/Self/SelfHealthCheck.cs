using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Granitos.Api.HealthChecks.Self;

public sealed class SelfHealthCheck : IHealthCheck
{
    public const string Name = "self";

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}