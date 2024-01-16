using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Granitos.Common.Testing;

public sealed record TestServerFixtureContext(
    IFeatureCollection Features,
    IServiceProvider Services);

public class TestServerFixture : IDisposable
{
    private IHost? _host;
    private TestServer? _server;
    private HttpMessageHandler? _handler;

    public TestServerFixture() { }

    public HttpMessageHandler Handler
    {
        get
        {
            EnsureTestServer();
            return _handler!;
        }
    }

    public IServiceProvider Services
    {
        get
        {
            EnsureTestServer();
            return _host!.Services;
        }
    }

    #region Configuration callbacks

    private Action<HostBuilderContext, IConfigurationBuilder>? _configureAppConfiguration;
    public void ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureAppConfiguration) => _configureAppConfiguration = configureAppConfiguration;

    private Action<IWebHostBuilder>? _configureWebHostDefaults;
    public void ConfigureWebHostDefaults(Action<IWebHostBuilder> configureWebHostDefaults) => _configureWebHostDefaults = configureWebHostDefaults;

    private Action<TestServerFixtureContext>? _onTestServerStarted;
    public void OnTestServerStarted(Action<TestServerFixtureContext>? onTestServerStarted) => _onTestServerStarted = onTestServerStarted;

    private Func<TestServerFixtureContext, Task>? _onTestServerStartedAsync;
    public void OnTestServerStartedAsync(Func<TestServerFixtureContext, Task>? onTestServerStartedAsync) => _onTestServerStartedAsync = onTestServerStartedAsync;

    #endregion

    public void Dispose()
    {
        _handler?.Dispose();
        _server?.Dispose();
        _host?.Dispose();
        GC.SuppressFinalize(this);
    }

    public void EnsureTestServer()
    {
        if (_host is not null)
            return;

        var builder = new HostBuilder()
            .ConfigureAppConfiguration((hostBuilderContext, config) =>
            {
                _configureAppConfiguration?.Invoke(hostBuilderContext, config);
            })
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseTestServer();

                _configureWebHostDefaults?.Invoke(webHostBuilder);
            });

        _host = builder.Start();
        _server = _host.GetTestServer();
        _handler = _server.CreateHandler();

        var testServerContext = new TestServerFixtureContext(
            _server.Features,
            _host.Services);

        _onTestServerStarted?.Invoke(testServerContext);
        _onTestServerStartedAsync?.Invoke(testServerContext).Wait();
    }
}