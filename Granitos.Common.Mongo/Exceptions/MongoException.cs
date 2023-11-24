namespace Granitos.Common.Mongo.Exceptions;

public class MongoException : Exception
{
    public MongoException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    public string Code { get; }
}