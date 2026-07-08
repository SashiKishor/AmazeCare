using System.Diagnostics;

namespace AmazeCareWebApi.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation(
            "Handling request started: {Method} {Path} TraceId: {TraceId}",
             context.Request.Method,
             context.Request.Path,
             context.TraceIdentifier);

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                _logger.LogInformation(
                    "HTTP Response completed: {Method} {Path} Status: {StatusCode} TraceId: {TraceId} Duration: {Duration}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode, 
                    context.TraceIdentifier,     
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
