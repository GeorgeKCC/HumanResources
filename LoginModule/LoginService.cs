using FluentValidation;
using LoginContract.Contract;
using LoginContract.Dtos.Requests;
using LoginModule.Features.CSRFFeature;
using LoginModule.Features.LoginFeature;
using Microsoft.Extensions.DependencyInjection;

namespace LoginModule
{
    public static class LoginService
    {
        public static IServiceCollection RegisterLoginService(this IServiceCollection services)
        {
            services.AddScoped<ILogin, Login>();
            services.AddScoped<ICSRF, CSRF>();

            services.AddScoped<IValidator<LoginRequest>, LoginValidator>();
            return services;
        }
    }
}
