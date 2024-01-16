using Granitos.Common.Errors.Http.Exceptions;

namespace Granitos.Common.Errors.FakeApi.Exceptions;

public class OperationNotImplementedException : HttpException
{
    public OperationNotImplementedException()
        : base("not_implemented", System.Net.HttpStatusCode.NotImplemented, "The operation is not implemented")
    {
    }
}