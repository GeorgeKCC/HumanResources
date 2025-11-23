using ManagementContract.Contracts;
using ManagementModule.Features.CreateActiveDeactiveFeature;
using ManagementModule.Features.CreateSecurityFeature;
using ManagementModule.Features.DeactivatedSecurityFeature;
using ManagementModule.Features.GetByEmailSecurityFeature;
using Microsoft.Extensions.DependencyInjection;
using Shared.Generics.Response;

namespace ManagementModule
{
    public static class ManagementService
    {
        public static IServiceCollection RegisterManagementServices(this IServiceCollection services)
        {
            services.AddScoped<IStrategySecurity, CreateSecurity>();
            services.AddScoped<IGetByEmailSecurity, GetByEmailSecurity>();
            services.AddScoped<IStrategySecurity, DeactivatedSecurity>();
            services.AddScoped<ICreateActiveDeactive<GenericResponse<bool>>, CreateActiveDeactive>();
            return services;
        }
    }
}
