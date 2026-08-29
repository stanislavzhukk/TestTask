using Application.Exceptions;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (ApiException ex)
            {
                _logger.LogInformation("Handled API exception: {Type} - {Message}", ex.GetType().Name, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.StatusCode;

                //List<string>? errors = new List<string>();
                //if (ex.Errors != null && ex.Errors.Count != 0)
                //{
                //    foreach (var error in ex.Errors)
                //    {
                //        errors.Add(error);
                //    }
                //}

                var response = new
                {
                    message = ex.Message,
                    //errors = errors.ToArray()
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new { message = "Internal server error" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}