using System.Net;
using FluentValidation;
using Granitos.Common.Errors.Core;
using Granitos.Common.Errors.Core.Builders;
using Granitos.Common.Errors.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Granitos.Common.Errors.FluentValidation;

internal class ValidationExceptionProblemDetailsFactory : ProblemDetailsFactory<ValidationException>
{
    public ValidationExceptionProblemDetailsFactory(IErrorDocsUrlFactory errorDocsUrlFactory, IHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor) : base(errorDocsUrlFactory, hostEnvironment, httpContextAccessor)
    {
    }

    protected override void OnBeforeBuild(ProblemDetailsBuilder builder, ValidationException exception)
    {
        builder
            .WithType(_errorDocsUrlFactory.Create("invalid_model"))
            .WithStatus((int)HttpStatusCode.BadRequest)
            .WithTitle("invalid_model")
            .WithExtension("errors", exception.Errors.Select(error => new
            {
                propertyName = error.PropertyName,
                errorCode = error.ErrorCode,
                errorMessage = error.ErrorMessage,
                attemptedValue = error.AttemptedValue,
            }));
    }
}