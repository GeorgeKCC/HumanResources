using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Exception.Handler;

namespace Shared
{
    public static class SharedService
    {
        public static void BuilderSharedModule(WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddExceptionHandler<ExceptionHandler>();
        }

        public static IServiceCollection ServicesSharedModule(this IServiceCollection service)
        {
            return service;
        }

        public static IApplicationBuilder ApplicationSharedModule(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(x => { });
            return app;
        }
    }
}
