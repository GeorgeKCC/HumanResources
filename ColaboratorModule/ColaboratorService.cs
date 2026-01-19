using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using ColaboratorModule.Features.AskWithRagColaboratorFeature;
using ColaboratorModule.Features.ColaboratorRedisFeature;
using ColaboratorModule.Features.CreateColaboratorFeature;
using ColaboratorModule.Features.GetAllColaboratorFeature;
using ColaboratorModule.Features.GetByEmailByColaboratorFeature;
using ColaboratorModule.Features.GetByIdColaboratorFeature;
using ColaboratorModule.Features.NotificationHubFeature;
using ColaboratorModule.Features.UpdateColaboratorFeature;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ColaboratorModule
{
    public static class ColaboratorService
    {
        public static IServiceCollection RegisterColaboratorServices(this IServiceCollection services)
        {
            services.AddScoped<ICreateColaborator, CreateColaborator>();
            services.AddScoped<IUpdateColaborator, UpdateColaborator>();
            services.AddScoped<IGetAllColaborator, GetAllColaborator>();
            services.AddScoped<IGetByIdColaborator, GetByIdColaborator>();
            services.AddScoped<IGetByEmailColaborator, GetByEmailColaborator>();
            services.AddScoped<IColaboratorNotificationHub, ColaboratorNotificationHub>();
            services.AddScoped<IColaboratorRedis, ColaboratorRedis>();
            services.AddScoped<IAskWithRagColaborator, AskWithRagColaborator>();

            services.AddScoped<IValidator<UpdateColaboratorRequest>, UpdateColaboratorValidator>();
            return services;
        }
    }
}