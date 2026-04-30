using AuthService.API.Endpoints;
using AuthService.API.Options;
using AuthService.API.Services;
using AuthService.Application;
using AuthService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthApplication();
builder.Services.AddAuthInfrastructure(builder.Configuration);
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .Validate(
        o => o.SigningKey.Length >= 32
             && !string.IsNullOrWhiteSpace(o.Issuer)
             && !string.IsNullOrWhiteSpace(o.Audience),
        "Jwt: SigningKey must be at least 32 characters; Issuer and Audience are required.")
    .ValidateOnStart();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapUserEndpoints();
app.MapAuthEndpoints();

await app.RunAsync();

public partial class Program;
