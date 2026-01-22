namespace Shared.hub
{
    public static class UseServiceSignalR
    {
        public static IServiceCollection AddServiceSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }

        /// <summary>
        /// Configuration url expose for signalr
        /// </summary>
        /// <param name="app"></param>
        /// <param name="url">Configuration url for example => "/hubs/notifications"</param>
        /// <returns></returns>
        public static WebApplication UseAppMapSignalR(this WebApplication app, string url)
        {
            app.MapHub<NotificationHub>(url).RequireAuthorization();
            return app;
        }
    }
}
