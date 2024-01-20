using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Errors.Http.Tests.Extensions;

internal static class HttpContextExtensions
{
    public static IServiceCollection AddHttpContextAccessorStub(this IServiceCollection services)
    {
        return services.AddSingleton<IHttpContextAccessor, HttpContextAccessorStub>();
    }

    internal class HttpContextAccessorStub : IHttpContextAccessor
    {
        public HttpContext? HttpContext { get; set; } = new HttpContextStub();
    }

    private class HttpContextStub : HttpContext
    {
        public override IFeatureCollection Features => throw new NotImplementedException();

        public override HttpRequest Request => throw new NotImplementedException();

        public override HttpResponse Response => throw new NotImplementedException();

        public override ConnectionInfo Connection => throw new NotImplementedException();

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        // public override AuthenticationManager Authentication => throw new NotImplementedException();

        public override ClaimsPrincipal User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IDictionary<object, object?> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Abort()
        {
            throw new NotImplementedException();
        }
    }
}