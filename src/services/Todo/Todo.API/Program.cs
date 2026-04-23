using Microsoft.EntityFrameworkCore;
using Todo.Infrastructure;
using Todo.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTodoInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();

await app.RunAsync();
