using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

const string TokenSecret = "SuperDuperSecretKey";
const string TenantIdClaimType = "tenantid";
const string ClientIdClaimType = "client_id";
const string PrefferedUserNameClaimType = "preferred_username";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSecret)),
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/health", (ILogger<Program> logger) =>
{
    logger.LogInformation("Health check endpoint.");
    return Results.Ok();
});

app.MapGet("/test", (ILogger<Program> logger) =>
{
    logger.LogDebug("DEBUG Test endpoint.");
    logger.LogInformation("INFO Test endpoint.");
    logger.LogWarning("WARNING Test endpoint.");
    return Results.Ok();
});

app.MapPost("/api/oauth/token", () =>
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSecret));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    var jwt = new JwtSecurityToken(
        signingCredentials: credentials,
        claims: new Claim[]
        {
            new Claim(TenantIdClaimType, "00000000-0000-0000-0000-000000000001"),
            new Claim(ClientIdClaimType, "yZy5wbVUXFjHhZwk7TZ5RACrPqANrU3H"),
            new Claim(PrefferedUserNameClaimType, "username@domain.com"),
        });

    var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

    return Results.Json(new
    {
        token_type = "Bearer",
        access_token = accessToken,
    });
});

app.MapPost("/api/oauth/test", [Authorize] () =>
{
    return Results.Ok();
});

app.Run();

