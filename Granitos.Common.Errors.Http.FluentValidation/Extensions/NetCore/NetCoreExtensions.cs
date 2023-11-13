using FluentValidation;
using Granitos.Common.Errors.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Errors.FluentValidation.Extensions.NetCore;

public static class NetCoreExtensions
{
    public static IServiceCollection AddHttpFluentValidationErrors(this IServiceCollection services)
    {
        return services
                .AddProblemDetailsFactory<ValidationException, ValidationExceptionProblemDetailsFactory>()
            ;
    }
}