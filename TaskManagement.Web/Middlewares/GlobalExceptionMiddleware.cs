using System.Net;
using System.Text.Json;

namespace TaskManagement.Web.Middlewares
{
    /// <summary>
    /// Middleware to globally handle unhandled exceptions and return appropriate error responses.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Middleware invocation logic for handling exceptions.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // proceed to next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Something went wrong. Please try again later.",
                    Detailed = ex.Message // optional: remove in production for security
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
