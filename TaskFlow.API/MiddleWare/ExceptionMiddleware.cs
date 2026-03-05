using System.Net;
using System.Text.Json;

namespace TaskFlow.API.MiddleWare;

public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // move to next middleware
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var result = JsonSerializer.Serialize(new
        {
            message = exception.Message,
            statusCode = context.Response.StatusCode
        });

        return context.Response.WriteAsync(result);
    }
}
