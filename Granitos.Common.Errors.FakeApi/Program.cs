using Granitos.Common.Errors.FakeApi.Exceptions;
using Granitos.Common.Errors.FluentValidation.Extensions.NetCore;
using Granitos.Common.Errors.Http.Extensions.AspNetCore;
using Granitos.Common.Errors.Http.Extensions.AspNetCore.Filters;
using Granitos.Common.Errors.Http.Extensions.NetCore;

var builder = WebApplication.CreateBuilder(args);

IReadOnlySet<Type> types = new HashSet<Type>()
{
  typeof(NotFoundException),
  typeof(HttpRequestException),
};

builder.Services
  .AddEndpointsApiExplorer()
  .AddSwaggerGen()
  .AddHttpContextAccessor()
  .AddHttpErrors()
  .AddHttpFluentValidationErrors()
  .AddHttpExceptionFilterOptions(new HttpExceptionFilterOptions
  {
    ExcludeExceptionLog = ex => types.Contains(ex.GetType()),
    ExcludeProblemDetailsLog = problemDetails => problemDetails.Status is >= 400 and <= 499,
  })
  .AddControllers(config =>
  {
    config.Filters.Add(typeof(HttpExceptionFilter));
  });

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();