namespace Backend.Api
{
    public class ExcepsionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExcepsionHandlerMiddleware> _logger;
        public ExcepsionHandlerMiddleware(RequestDelegate next, ILogger<ExcepsionHandlerMiddleware> logger)
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
                _logger.LogError(ex, "Необработанная ошибка");
                
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain";

                    await context.Response.WriteAsync("На сервере произошла ошибка при обработке запроса.");
                }  
            }
        }
    }

    public static class ExcepsionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExcepsionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExcepsionHandlerMiddleware>();
        }
    }
}
