using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using ColaboratorModule.Data.Context;
using ColaboratorModule.Features.CreateColaboratorFeature;
using ColaboratorModule.Features.GetAllColaboratorFeature;
using ColaboratorModule.Features.UpdateColaboratorFeature;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ColaboratorModule
{
    public static class ColaboratorService
    {
        public static IServiceCollection RegisterColaboratorServices(this IServiceCollection services, IConfiguration configuration)
        {
            var database = configuration.GetConnectionString("Database");
            services.AddDbContext<ColaboratorContext>(x =>
            {
                x.UseSqlServer(database);
            });

            services.AddScoped<ICreateColaborator, CreateColaborator>();
            services.AddScoped<IUpdateColaborator, UpdateColaborator>();
            services.AddScoped<IGetAllColaborator, GetAllColaborator>();
            services.AddScoped<IValidator<UpdateColaboratorRequest>, UpdateColaboratorValidator>();
            return services;
        }
    }
}