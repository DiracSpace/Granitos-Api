using Granitos.Common.Errors.Http.Exceptions;

namespace Granitos.Common.Errors.FakeApi.Exceptions;

public class NotFoundException : HttpException
{
    public NotFoundException()
        : base("not_found", System.Net.HttpStatusCode.NotFound, "The resource was not found")
    {
    }
}