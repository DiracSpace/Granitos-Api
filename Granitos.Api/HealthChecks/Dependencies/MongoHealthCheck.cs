using Granitos.Common.Mongo.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Granitos.Api.HealthChecks.Dependencies;

/// <summary>
/// Source code extracted and adapted from:
/// https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/src/HealthChecks.MongoDb/MongoDbHealthCheck.cs
/// </summary>
public sealed class MongoHealthCheck : IHealthCheck
{
    public const string Name = "Mongo";

    private readonly IOptions<MongoOptions> _options;

    public MongoHealthCheck(IOptions<MongoOptions> options)
    {
        _options = options;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = new MongoClient(_options.Value.ConnectionString);
            await PingServerAsync(client, cancellationToken);
            await PingDatabaseAsync(client, cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }

    private static async Task PingServerAsync(IMongoClient client, CancellationToken cancellationToken)
    {
        using var cursor = await client.ListDatabaseNamesAsync(cancellationToken);
        await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    private async Task PingDatabaseAsync(IMongoClient client, CancellationToken cancellationToken)
    {
        var database = client.GetDatabase(_options.Value.DatabaseName);
        var pingCommand = new BsonDocumentCommand<BsonDocument>(BsonDocument.Parse("{ ping: 1 }"));
        await database.RunCommandAsync(pingCommand, cancellationToken: cancellationToken);
    }
}