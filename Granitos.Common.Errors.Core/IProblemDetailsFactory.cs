namespace Granitos.Common.Errors.Core;

public interface IProblemDetailsFactory
{
}

public interface IProblemDetailsFactory<TException> : IProblemDetailsFactory
    where TException : Exception
{
    public ProblemDetails From(TException exception);
}