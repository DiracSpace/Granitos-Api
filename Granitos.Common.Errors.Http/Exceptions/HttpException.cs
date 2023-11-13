using System.Net;

namespace Granitos.Common.Errors.Http.Exceptions;

public class HttpException : Exception
{
    public string Code { get; }
    public HttpStatusCode Status { get; }
    public override Dictionary<string, object?> Data { get; } = new();

    public HttpException(string code, HttpStatusCode status)
    {
        Code = code;
        Status = status;
    }

    public HttpException(string code, HttpStatusCode statusCode, string message)
        : base(message)
    {
        Code = code;
        Status = statusCode;
    }

    public HttpException(string code, HttpStatusCode statusCode, string message, Exception? innerException)
        : base(message, innerException)
    {
        Code = code;
        Status = statusCode;
    }

    public HttpException WithData(string key, object? value)
    {
        Data.Add(key, value);
        return this;
    }

    public HttpException WithData(KeyValuePair<string, object?> pair)
    {
        return WithData(pair.Key, pair.Value);
    }

    public HttpException WithData(IDictionary<string, object?> data)
    {
        foreach (var pair in data)
            WithData(pair);

        return this;
    }
}
