namespace Granitos.Common.Errors.Core.Builders;

public class ProblemDetailsBuilder
{
    private readonly ProblemDetails _problemDetails;

    public ProblemDetailsBuilder()
    {
        _problemDetails = new ProblemDetails();
    }

    public ProblemDetailsBuilder WithType(string type)
    {
        _problemDetails.Type = type;
        return this;
    }

    public ProblemDetailsBuilder WithStatus(int status)
    {
        _problemDetails.Status = status;
        return this;
    }

    public ProblemDetailsBuilder WithTitle(string title)
    {
        _problemDetails.Title = title;
        return this;
    }

    public ProblemDetailsBuilder WithDetails(string details)
    {
        _problemDetails.Details = details;
        return this;
    }

    public ProblemDetailsBuilder WithInstance(string instance)
    {
        _problemDetails.Instance = instance;
        return this;
    }

    public ProblemDetailsBuilder WithExtension(string key, object? value)
    {
        if (_problemDetails.Extensions.ContainsKey(key))
            // Prevent duplicate key exception by removing existing key
            _problemDetails.Extensions.Remove(key);

        _problemDetails.Extensions.Add(key, value);

        return this;
    }

    public ProblemDetailsBuilder WithExtensions(Dictionary<string, object?> extension)
    {
        foreach (var pair in extension)
            WithExtension(pair.Key, pair.Value);

        return this;
    }

    public ProblemDetails Build()
    {
        return _problemDetails;
    }
}