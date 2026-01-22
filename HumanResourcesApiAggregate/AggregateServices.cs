namespace HumanResourcesApiAggregate
{
    public static class AggregateServices
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.MapperBinding(configuration);
            services.AddServiceExceptionHandler();
            services.AddServiceHttpContextAccesor();
            services.AddServiceDatabaseContext(configuration);
            services.AddServiceHybridCache(configuration);
            services.AddServiceRedLock();
            services.AddServiceCors(configuration);
            services.AddServiceAntiforgeryValidate();
            services.AddServiceAuthentication(configuration);
            services.AddServiceRateLimit();
            services.AddServiceLogginDelegate();
            services.AddServiceSignalR();
            services.AddServiceDataProtection();
            services.AddServiceQdrant(configuration);
            services.AddServiceOllama(configuration);
            services.AddServiceRabbitMQ(configuration);

            //Modules
            services.RegisterColaboratorServices();
            services.RegisterManagementServices(configuration);
            services.RegisterLoginService();

            return services;
        }

        public static WebApplication RegisterApp(this WebApplication app)
        {
            app.UseAppCors();
            app.UseAppAntiforgery();
            app.UseAppMiddlewareCorrelationId();
            app.UseAppRedLockMiddleware();
            app.UseAppExceptionHandler();
            app.UseAppUseAuthenticationAndAuthorization();
            app.UseAppMapSignalR("/hubs/notifications");
            app.UseAppRateLimit();

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            exception = entry.Value.Exception?.Message,
                            duration = entry.Value.Duration.ToString()
                        })
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            });

            return app;
        }

        public static ConfigureHostBuilder RegisterHost(this ConfigureHostBuilder host)
        {
            host.UseHostSerilog();
            host.UseDefaultServiceProvider(options =>
            {
                options.ValidateOnBuild = true;
                options.ValidateScopes = true;
            });

            return host;
        }
    }
}
