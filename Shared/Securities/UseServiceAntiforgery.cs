namespace Shared.Securities
{
    public static class UseServiceAntiforgery
    {
        /// <summary>
        /// Set X-XSRF-TOKEN in header value for methods POST, PUT or PATCH
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceAntiforgeryValidate(this IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = CSRF_Constant.KEY;
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.HeaderName = CSRF_Constant.KEY;
            });

            services.AddScoped<IGetAndStoreTokensAntiforgery, GetAndStoreTokensAntiforgery>();

            return services;
        }

        public static IApplicationBuilder UseAppAntiforgery(this IApplicationBuilder app)
        {
            app.UseAntiforgery();

            return app;
        }
    }
}
