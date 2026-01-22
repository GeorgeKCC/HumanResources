namespace Shared.MappersBinding
{
    public static class UseServiceMapperBinding
    {
        public static IServiceCollection MapperBinding(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenConfiguration>(configuration.GetSection("Token"));

            return services;
        }
    }
}
