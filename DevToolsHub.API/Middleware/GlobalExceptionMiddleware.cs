using System.Text.Json;
using DevToolsHub.Core.DTOs;
using DevToolsHub.Core.Exceptions;

namespace DevToolsHub.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next,ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An unhandled exception occurred.");

                await HandleExceptionAsync(context,ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context,Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = 500;
            string message = "Something went wrong.";

            if (exception is NotFoundException)
            {
                statusCode = 404;
                message = exception.Message;
            }
            else if (exception is BadRequestException)
            {
                statusCode = 400;
                message = exception.Message;
            }
            else if (exception is UnauthorizedException)
            {
                statusCode = 401;
                message = exception.Message;
            }

            context.Response.StatusCode = statusCode;

            var response = new ErrorResponseDto
            {
                Success = false,
                Message = message,
                Errors = new List<string>()
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}