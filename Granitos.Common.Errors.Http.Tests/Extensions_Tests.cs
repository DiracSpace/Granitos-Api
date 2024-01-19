using Granitos.Common.Errors.Core;
using Granitos.Common.Errors.Core.Extensions;
using Granitos.Common.Errors.Http.Exceptions;
using Granitos.Common.Errors.Http.Extensions.NetCore;
using Granitos.Common.Errors.Http.Tests.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Granitos.Common.Errors.Http.Tests;

public class Extensions_Tests
{
    [Fact]
    public void CanRegisterBuiltInFactories()
    {
        var services = new ServiceCollection()
            .AddConfigurationStub()
            .AddHostEnvironmentStub()
            .AddHttpContextAccessorStub()
            .AddHttpErrors()
            .BuildServiceProvider();

        var factories = services
            .GetServices<IProblemDetailsFactory>()
            .OrderBy(x => x.GetType().Name)
            .ToList();

        factories.Count.ShouldBe(2);
    }

    [Fact]
    public void CanGetFactoryByGenericToken()
    {
        var services = new ServiceCollection()
            .AddConfigurationStub()
            .AddHostEnvironmentStub()
            .AddHttpContextAccessorStub()
            .AddHttpErrors()
            .BuildServiceProvider();

        var apiExceptionFactory = services.GetRequiredService<IProblemDetailsFactory<HttpException>>();
        apiExceptionFactory.ShouldNotBeNull();

        var exceptionFactory = services.GetRequiredService<IProblemDetailsFactory<Exception>>();
        exceptionFactory.ShouldNotBeNull();
    }

    [Fact]
    public void CanRegisterCustomFactory()
    {
        var services = new ServiceCollection()
            .AddConfigurationStub()
            .AddHostEnvironmentStub()
            .AddHttpContextAccessorStub()
            .AddHttpErrors()
            .AddProblemDetailsFactory<CustomException, CustomExceptionFactory>()
            .BuildServiceProvider();

        var factory = services.GetRequiredService<IProblemDetailsFactory<CustomException>>();
        factory.ShouldNotBeNull();
    }
}