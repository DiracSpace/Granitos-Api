using System.Net;
using Granitos.Common.Errors.Core;
using Granitos.Common.Errors.Core.Builders;
using Granitos.Common.Errors.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Granitos.Common.Errors.Http;

public class ProblemDetailsFactory<TException> : IProblemDetailsFactory<TException>
    where TException : Exception
{
    protected readonly IErrorDocsUrlFactory _errorDocsUrlFactory;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProblemDetailsFactory(IErrorDocsUrlFactory errorDocsUrlFactory, IHostEnvironment hostEnvironment,
        IHttpContextAccessor httpContextAccessor)
    {
        _errorDocsUrlFactory = errorDocsUrlFactory;
        _hostEnvironment = hostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    private bool IsEnvironmentLocalhost => _hostEnvironment.EnvironmentName == "localhost";

    private bool IsEnvironmentDevelopment => _hostEnvironment.EnvironmentName == "Development";

    public virtual ProblemDetails From(TException exception)
    {
        var builder = new ProblemDetailsBuilder()
            .WithType(_errorDocsUrlFactory.Create("unknown"))
            .WithStatus((int)HttpStatusCode.InternalServerError)
            .WithTitle("unknown")
            .WithDetails(exception.GetMessages());

        if (_httpContextAccessor.HttpContext is not null)
            builder.WithInstance(_httpContextAccessor.HttpContext.Request.Path);

        if (IsEnvironmentLocalhost || IsEnvironmentDevelopment)
            IncludeStackTraces(builder, exception);

        if (TryGetTraceId(out var traceId))
            builder.WithExtension("traceId", traceId);

        OnBeforeBuild(builder, exception);

        return builder.Build();
    }

    protected virtual void OnBeforeBuild(ProblemDetailsBuilder builder, TException exception)
    {
        // Allow inherited classes to override for customization
    }

    private static void IncludeStackTraces(ProblemDetailsBuilder builder, TException exception)
    {
        builder.WithExtension("stackTrace", exception.StackTrace);

        if (exception.InnerException?.StackTrace is not null)
            builder.WithExtension("innerStackTrace", exception.InnerException.StackTrace);
    }

    private bool TryGetTraceId(out string traceId)
    {
        var requestTraceId = _httpContextAccessor
            .HttpContext
            .Request
            .GetTraceIdOrDefault();

        if (!string.IsNullOrWhiteSpace(requestTraceId))
        {
            traceId = requestTraceId;
            return true;
        }

        traceId = null!;
        return false;
    }
}

internal static class HttpRequestExtensions
{
    /// <summary>
    ///     The key of the trace parent header.
    /// </summary>
    private const string TraceParentHeader = "traceparent";

    /// <summary>
    ///     A valid trace parent should contain exactly 4 parts, e.g. 00-00c5603af68757ae581b2b4763a4d593-b441b6f9498984d0-01.
    /// </summary>
    private const int TraceParentParts = 4;

    /// <summary>
    ///     The traceId is the second part (index 1) of a trace parent header.
    /// </summary>
    private const int TraceIdIndex = 1;

    public static string? GetTraceIdOrDefault(this HttpRequest request)
    {
        var traceParentHeader = request.Headers
            .FirstOrDefault(x => x.Key == TraceParentHeader);

        var traceParentValue = traceParentHeader.Value.ToString();

        if (string.IsNullOrWhiteSpace(traceParentValue))
            return null;

        var traceParentParts = traceParentValue.Split('-');

        if (traceParentParts.Length != TraceParentParts)
            return null;

        var traceId = traceParentParts[TraceIdIndex];

        return string.IsNullOrWhiteSpace(traceId) ? null : traceId;
    }
}