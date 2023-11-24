using Granitos.Common.Errors.Core;
using Granitos.Common.Errors.Core.Builders;
using Granitos.Common.Errors.Http.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Granitos.Common.Errors.Http;

internal class HttpExceptionProblemDetailsFactory : ProblemDetailsFactory<HttpException>,
    IProblemDetailsFactory<HttpException>
{
    public HttpExceptionProblemDetailsFactory(
        IErrorDocsUrlFactory errorDocsUrlFactory,
        IHostEnvironment hostEnvironment,
        IHttpContextAccessor httpContextAccessor)
        : base(errorDocsUrlFactory, hostEnvironment, httpContextAccessor)
    {
    }

    protected override void OnBeforeBuild(ProblemDetailsBuilder builder, HttpException exception)
    {
        builder
            .WithType(_errorDocsUrlFactory.Create(exception.Code))
            .WithStatus((int)exception.Status)
            .WithTitle(exception.Code)
            .WithExtensions(exception.Data);
    }
}