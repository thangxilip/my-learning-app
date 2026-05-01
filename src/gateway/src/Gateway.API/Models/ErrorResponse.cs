namespace Gateway.API.Models;

public class ErrorResponse
{
    public string TraceId { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string? Details { get; set; }
}