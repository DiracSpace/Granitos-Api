namespace Granitos.Common.Errors.Core.Resolvers;

public interface IProblemDetailsResolver
{
    ProblemDetails Resolve<TException>(TException exception)
        where TException : Exception;
}

internal class ProblemDetailsResolver : IProblemDetailsResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ProblemDetailsResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ProblemDetails Resolve<TException>(TException exception)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(exception, nameof(exception));
        var factory = GetFactory(exception.GetType());
        var problemDetails = GetProblemDetails(factory, exception);
        return problemDetails;
    }

    private object GetFactory(Type exceptionType)
    {
        while (true)
        {
            var factoryType = typeof(IProblemDetailsFactory<>).MakeGenericType(exceptionType); // Equivalent to "IProblemDetailsFactory<TException>"
            var factory = _serviceProvider.GetService(factoryType);

            if (factory is not null) return factory;

            if (exceptionType.BaseType is null) return GetDefaultFactory();
            exceptionType = exceptionType.BaseType;
        }
    }

    private object GetDefaultFactory()
    {
        var factoryType = typeof(IProblemDetailsFactory<>).MakeGenericType(typeof(Exception)); // Equivalent to "IProblemDetailsFactory<TException>"
        var factory = _serviceProvider.GetService(factoryType);

        if (factory is not null)
            return factory;

        throw new Exception($"Could not find default {nameof(IProblemDetailsFactory<Exception>)} of type {nameof(Exception)}");
    }

    private static ProblemDetails GetProblemDetails<TException>(object problemDetailsFactory, TException exception)
        where TException : Exception
    {
        var problemDetails = problemDetailsFactory
            ?.GetType()
            ?.GetMethod(nameof(IProblemDetailsFactory<TException>.From))
            ?.Invoke(problemDetailsFactory, new object?[] { exception });

        if (problemDetails is not null)
            return (ProblemDetails)problemDetails;

        throw new InvalidOperationException(@$"Could not get problem details using factory ""{problemDetailsFactory!.GetType().Name}"" and exception ""{exception.GetType().Name}"".");
    }
}
