using Granitos.Common.Errors.Core;
using Granitos.Common.Errors.Core.Extensions;
using Granitos.Common.Errors.Http.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Errors.Http.Extensions.NetCore;

public static class NetCoreExtensions
{
    public static IServiceCollection AddHttpErrors(this IServiceCollection services)
    {
        return services
                .AddSingleton<IErrorDocsUrlFactory, ErrorDocsUrlFactory>()
                .AddProblemDetailsFactory<Exception, ExceptionProblemDetailsFactory>()
                .AddProblemDetailsFactory<HttpException, HttpExceptionProblemDetailsFactory>()
                .AddProblemDetailsResolver()
            ;
    }
}