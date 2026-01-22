namespace Shared.HttpContextAccessor
{
    public static class UseServiceHttpContextAccessor
    {
        public static IServiceCollection AddServiceHttpContextAccesor(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
