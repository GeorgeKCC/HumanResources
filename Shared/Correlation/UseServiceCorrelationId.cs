namespace Shared.Correlation
{
    public static class UseServiceCorrelationId
    {
        public static IApplicationBuilder UseAppMiddlewareCorrelationId(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationMiddleware>();

            return app;
        }
    }
}
