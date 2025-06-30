using System.Net;
using System.Text.Json;
using SmartHome.Application.Common;
using SmartHome.Application.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unhandled exception: {ex.Message}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        string message;

        if (exception is NotFoundException)
        {
            statusCode = (int)HttpStatusCode.NotFound;
            message = exception.Message;
        }
        else if (exception is BadRequestException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            message = exception.Message;
        }
        else if (exception is ForbiddenException)
        {
            statusCode = (int)HttpStatusCode.Forbidden;
            message = exception.Message;
        }
        else if (exception is ConflictException)
        {
            statusCode = (int)HttpStatusCode.Conflict;
            message = exception.Message;
        }
        else if (exception is UnauthorizedException)
        {
            statusCode = (int)HttpStatusCode.Unauthorized;
            message = exception.Message;
        }

        else
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
            message = "Internal server error"; 
        }


        var response = new ApiResponse(statusCode, message, null);
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
