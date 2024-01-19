using Granitos.Common.Errors.Core;

namespace Granitos.Common.Errors.Http.Tests;

internal class CustomExceptionFactory : IProblemDetailsFactory<CustomException>
{
    public ProblemDetails From(CustomException exception)
    {
        return new ProblemDetails();
    }
}