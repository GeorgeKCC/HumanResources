namespace Shared.Exception
{
    public static class UseServiceExceptionHandler
    {
        public static IServiceCollection AddServiceExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<ExceptionHandler>();

            return services;
        }

        public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(x => { });

            return app;
        }
    }
}
