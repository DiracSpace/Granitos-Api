using Granitos.Common.Errors.Http.Exceptions;

namespace Granitos.Common.Errors.FakeApi.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException()
        : base("bad_request", System.Net.HttpStatusCode.BadRequest, "The payload was incorrect")
    {
    }
}