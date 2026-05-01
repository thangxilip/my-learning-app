using Flashcard.API.Auth;
using Flashcard.API.Middleware;
using Flashcard.Application;
using Flashcard.Infrastructure;
using Flashcard.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFlashcardApplication();
builder.Services.AddFlashcardInfrastructure(builder.Configuration);

// Identity comes from X-User-Id / X-User-Email headers injected by the API gateway after JWT validation.
// Do not expose this service directly to clients without network isolation (only the gateway should reach it).
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = GatewayHeaderAuthenticationDefaults.SchemeName;
        options.DefaultChallengeScheme = GatewayHeaderAuthenticationDefaults.SchemeName;
    })
    .AddScheme<AuthenticationSchemeOptions, GatewayHeaderAuthenticationHandler>(
        GatewayHeaderAuthenticationDefaults.SchemeName,
        _ => { });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // await using var scope = app.Services.CreateAsyncScope();
    // var db = scope.ServiceProvider.GetRequiredService<FlashcardDbContext>();
    // await db.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ApiExceptionMiddleware>();
app.MapControllers();

await app.RunAsync();

public partial class Program;
