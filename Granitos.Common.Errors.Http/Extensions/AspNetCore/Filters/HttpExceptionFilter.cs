using Granitos.Common.Errors.Core.Resolvers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Granitos.Common.Errors.Http.Extensions.AspNetCore.Filters;

public class HttpExceptionFilter : IExceptionFilter
{
    private readonly ILogger<HttpExceptionFilter> _logger;
    private readonly IProblemDetailsResolver _problemDetailsResolver;
    private readonly IServiceProvider _serviceProvider;

    public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger, IProblemDetailsResolver problemDetailsResolver, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _problemDetailsResolver = problemDetailsResolver;
        _serviceProvider = serviceProvider;
    }

    private readonly IHttpExceptionFilterOptions _fallbackOptions = new HttpExceptionFilterOptions();
    private IHttpExceptionFilterOptions Options => _serviceProvider.GetService<IHttpExceptionFilterOptions>() ?? _fallbackOptions;

    public void OnException(ExceptionContext context)
    {
        if (!ExcludeExceptionLog(context.Exception))
        {
            _logger.LogError(context.Exception, "Error while processing the request: {Method} {Path}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path);
        }

        var problemDetails = _problemDetailsResolver.Resolve(context.Exception);

        if (!ExcludeProblemDetailsLog(problemDetails))
        {
            _logger.LogInformation(
                "Problem details: {ProblemDetailsJson}", 
                JsonConvert.SerializeObject(problemDetails, Formatting.Indented));
        }

        context.Result = new JsonResult(problemDetails)
        {
            StatusCode = problemDetails.Status,
        };

        context.ExceptionHandled = true;
    }

    private bool ExcludeExceptionLog(Exception exception)
    {
        return Options.ExcludeExceptionLog is null
            ? false
            : Options.ExcludeExceptionLog(exception);
    }

    private bool ExcludeProblemDetailsLog(Core.ProblemDetails problemDetails)
    {
        return Options.ExcludeProblemDetailsLog is null
            ? false
            : Options.ExcludeProblemDetailsLog(problemDetails);
    }
}