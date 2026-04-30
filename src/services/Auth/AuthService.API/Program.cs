using AuthService.API.Endpoints;
using AuthService.Application;
using AuthService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthApplication();
builder.Services.AddAuthInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapUserEndpoints();

await app.RunAsync();

public partial class Program;
