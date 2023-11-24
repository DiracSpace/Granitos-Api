using Granitos.Api.HealthChecks.DependencyInjection;
using Granitos.Common.Errors.FluentValidation.Extensions.NetCore;
using Granitos.Common.Errors.Http.Extensions.AspNetCore;
using Granitos.Common.Errors.Http.Extensions.AspNetCore.Filters;
using Granitos.Common.Errors.Http.Extensions.NetCore;
using Granitos.Common.Extensions;
using Granitos.Common.Mongo.DependencyInjection;
using Granitos.Services.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;

const string defaultCorsPolicy = "DefaultCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .ConfigureKestrel(options => options.AddServerHeader = false);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddEnvironmentVariables();

// Add Services to container
builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection("mongo"));

builder.Services.AddControllers();
builder.Services.AddCustomHealthCheckServices();
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


IReadOnlySet<Type> types = new HashSet<Type>()
{
    typeof(HttpRequestException),
};

builder.Services
    .AddHttpContextAccessor()
    .AddHttpErrors()
    .AddHttpFluentValidationErrors()
    .AddHttpExceptionFilterOptions(new HttpExceptionFilterOptions
    {
        ExcludeExceptionLog = ex => types.Contains(ex.GetType())
    })
    .AddControllers(config =>
    {
        config.Filters.Add(typeof(HttpExceptionFilter));
    });

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()
    || app.Environment.IsLocalhost())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(defaultCorsPolicy);

app.UseAuthorization();

app.MapHealthCheckEndpoints();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();