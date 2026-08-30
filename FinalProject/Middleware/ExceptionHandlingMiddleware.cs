using System.Net;
using System.Text.Json;

namespace FinalProject.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            InvalidOperationException => ((int)HttpStatusCode.BadRequest, exception.Message),
            KeyNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, exception.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;

        var response = new { message };
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}