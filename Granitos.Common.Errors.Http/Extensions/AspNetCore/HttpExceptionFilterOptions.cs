using Granitos.Common.Errors.Core;

namespace Granitos.Common.Errors.Http.Extensions.AspNetCore;

public interface IHttpExceptionFilterOptions
{
    public Func<Exception, bool>? ExcludeExceptionLog { get; }
    public Func<ProblemDetails, bool>? ExcludeProblemDetailsLog { get; }
} 

public class HttpExceptionFilterOptions : IHttpExceptionFilterOptions
{
    public Func<Exception, bool>? ExcludeExceptionLog { get; set; }
    public Func<ProblemDetails, bool>? ExcludeProblemDetailsLog { get; set; }
}