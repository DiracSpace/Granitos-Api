using Granitos.Common.Errors.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Granitos.Common.Errors.Http;

internal class ErrorDocsUrlFactory : IErrorDocsUrlFactory
{
    private const string DefaultTopLevelDomain = "dev";
    private const string FallbackDocsUrlTemplate = "https://apps.edgraph.{{TopLevelDomain}}/docs/errors#{{ErrorCode}}";
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;

    private readonly Dictionary<string, string> _topLevelDomainMappings = new()
    {
        ["localhost"] = "dev",
        ["development"] = "dev",
        ["production"] = "com"
    };

    public ErrorDocsUrlFactory(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
    }

    private string TopLevelDomain
    {
        get
        {
            var environment = _hostEnvironment.EnvironmentName.Trim().ToLower();

            return _topLevelDomainMappings.GetValueOrDefault(environment)
                   ?? DefaultTopLevelDomain;
        }
    }

    public string Create(string errorCode)
    {
        var docsUrlTemplate = _configuration["Errors:DocsUrl"]
                              ?? FallbackDocsUrlTemplate;

        return docsUrlTemplate
            .Replace("{{TopLevelDomain}}", TopLevelDomain)
            .Replace("{{ErrorCode}}", errorCode);
    }
}