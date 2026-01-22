using Google.Protobuf.WellKnownTypes;

namespace Shared.Cors
{
    public static class UseServiceCors
    {
        /// <summary>
        /// Configuration in appsetting.json this sección code => "UrlCors": ["this section add url","this section add url2","this section add url3", ...],
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Ex"></exception>
        public static IServiceCollection AddServiceCors(this IServiceCollection services, IConfiguration configuration)
        {
            var urlCors = configuration.GetSection("UrlCors").Get<string[]>() ?? throw new Ex("Not implement configuration cors");
            services.AddCors(x =>
            {
                x.AddPolicy("CorsPolicy", z =>
                {
                    z.WithOrigins(urlCors);
                    z.AllowAnyHeader();
                    z.AllowAnyMethod();
                    z.AllowCredentials();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");

            return app;
        }
    }
}
