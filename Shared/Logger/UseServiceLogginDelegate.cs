namespace Shared.Logger
{
    public static class UseServiceLogginDelegate
    {
        public static IServiceCollection AddServiceLogginDelegate(this IServiceCollection services)
        {
            services.AddTransient<LoggingDelegatingHandler>();
            return services;
        }

        public static ConfigureHostBuilder UseHostSerilog(this ConfigureHostBuilder host)
        {
            host.UseSerilog(SeriLogger.Configure);

            return host;
        }
    }
}
