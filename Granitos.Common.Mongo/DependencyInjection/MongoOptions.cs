using System.ComponentModel;

namespace Granitos.Common.Mongo.DependencyInjection;

public sealed record MongoOptions
{
    public const string SectionName = "mongo";
    
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}