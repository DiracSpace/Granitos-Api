using Granitos.Common.Errors.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Granitos.Common.Errors.Http;

internal class ExceptionProblemDetailsFactory : ProblemDetailsFactory<Exception>, IProblemDetailsFactory<Exception>
{
    public ExceptionProblemDetailsFactory(
        IErrorDocsUrlFactory errorDocsUrlFactory,
        IHostEnvironment hostEnvironment,
        IHttpContextAccessor httpContextAccessor)
        : base(errorDocsUrlFactory, hostEnvironment, httpContextAccessor)
    {
    }
}