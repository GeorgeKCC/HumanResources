using ManagementContract.Contracts;
using ManagementModule.Features.CreateSecurityFeature;
using ManagementModule.Features.GetByEmailSecurityFeature;
using Microsoft.Extensions.DependencyInjection;

namespace ManagementModule
{
    public static class ManagementService
    {
        public static IServiceCollection RegisterManagementServices(this IServiceCollection services)
        {
            services.AddScoped<ICreateSecurity, CreateSecurity>();
            services.AddScoped<IGetByEmailSecurity, GetByEmailSecurity>();
            return services;
        }
    }
}
