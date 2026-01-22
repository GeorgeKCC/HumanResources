namespace Shared.RedLock
{
    public static class UseServiceRedLock
    {
        public static IServiceCollection AddServiceRedLock(this IServiceCollection services)
        {
            services.AddSingleton<RedLockFactory>(sp =>
            {
                var mux = sp.GetRequiredService<IConnectionMultiplexer>();
                if (mux == null || !mux.IsConnected)
                    throw new InvalidOperationException("Redis connection is not available for RedLockFactory.");

                return RedLockFactory.Create(
                [
                    new RedLockMultiplexer(mux)
                ]);
            });

            return services;
        }

        public static IApplicationBuilder UseAppRedLockMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RedLockMiddleware>();

            return app;
        }
    }
}
