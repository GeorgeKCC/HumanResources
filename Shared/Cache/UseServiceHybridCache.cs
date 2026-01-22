namespace Shared.Cache
{
    public static class UseServiceHybridCache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Ex"></exception>
        public static IServiceCollection AddServiceHybridCache(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Redis") ?? throw new Ex("Not found connection redis");

            services.AddMemoryCache();

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(connectionString));

            services.AddHybridCache(options =>
            {
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(5),
                    LocalCacheExpiration = TimeSpan.FromMinutes(1)
                };
            });

            services.AddHealthChecks()
                    .AddRedis(
                        connectionString,
                        "Redis Health",
                        HealthStatus.Degraded);

            return services;
        }
    }
}
