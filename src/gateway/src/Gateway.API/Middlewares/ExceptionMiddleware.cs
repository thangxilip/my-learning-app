using System.Net;
using System.Text.Json;
using Gateway.API.Models;

namespace Gateway.API.Middlewares;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var traceId = context.TraceIdentifier;

        // Log full details internally
        logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", traceId);

        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            TraceId = traceId,
            Message = env.IsDevelopment()
                ? ex.Message
                : "An unexpected error occurred.",
            Details = env.IsDevelopment()
                ? ex.StackTrace
                : null
        };

        context.Response.StatusCode = ex switch
        {
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}