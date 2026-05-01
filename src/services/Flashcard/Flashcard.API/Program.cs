using Flashcard.API.Extensions;
using Flashcard.API.Middleware;
using Flashcard.Application;
using Flashcard.Infrastructure;
using Flashcard.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFlashcardApplication();
builder.Services.AddFlashcardInfrastructure(builder.Configuration);

// With Jwt:* configured: Bearer is validated locally; otherwise only X-User-Id (gateway) is trusted.
builder.Services.AddFlashcardAuthentication(builder.Configuration);

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
