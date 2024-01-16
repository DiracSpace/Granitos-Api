using Granitos.Common.Errors.FakeApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Granitos.Common.Errors.FakeApi.Controllers;

[ApiController]
[Route("http")]
public class HttpController : ControllerBase
{
    public HttpController()
    {
    }

    [HttpPost("throw/bad-request")]
    public void ThrowBadRequest() => throw new BadRequestException();

    [HttpPost("throw/not-found")]
    public void ThrowNotFoundException() => throw new NotFoundException();

    [HttpPost("throw/not-implemented")]
    public void ThrowOperationNotImplementedException() => throw new OperationNotImplementedException();

    [HttpPost("throw/unknown")]
    public void ThrowUnknownException() => throw new ApplicationException();
}