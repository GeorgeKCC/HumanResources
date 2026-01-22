namespace Shared.RateLimit
{
    public static class UseServiceRateLimit
    {
        public static IServiceCollection AddServiceRateLimit(this IServiceCollection services,
                                                      int permitLimit = 100,
                                                      int windowMinute = 1,
                                                      int QueueLimit = 0)
        {
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("ip-policy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: key => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = permitLimit,
                            Window = TimeSpan.FromMinutes(windowMinute),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = QueueLimit
                        }));
            });

            return services;
        }

        public static IApplicationBuilder UseAppRateLimit(this IApplicationBuilder app)
        {
            app.UseRateLimiter();

            return app;
        }
    }
}
