using LoginContract.Contract;
using LoginModule.Features.LoginFeature;
using Microsoft.Extensions.DependencyInjection;

namespace LoginModule
{
    public static class LoginService
    {
        public static IServiceCollection RegisterLoginService(this IServiceCollection services)
        {
            services.AddScoped<ILogin, Login>();
            return services;
        }
    }
}
