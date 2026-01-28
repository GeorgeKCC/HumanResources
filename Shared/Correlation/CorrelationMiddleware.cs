namespace Shared.Correlation
{
    internal class CorrelationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault()
                                ?? Guid.NewGuid().ToString("N");
            context.Response.Headers["X-Correlation-Id"] = correlationId;
            using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}
