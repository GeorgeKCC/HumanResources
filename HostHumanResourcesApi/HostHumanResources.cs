using ColaboratorModule;
using LoginModule;
using ManagementModule;
using Microsoft.AspNetCore.Builder;
using Shared;
using Shared.Context;
using Shared.hub;
using Shared.RabbitMQ;

namespace HostHumanResourcesApi
{
    public static class HostHumanResources
    {
        public static void UseRegisterService(WebApplicationBuilder builder)
        {
            builder.Services.AddServiceRabbitMQ(builder.Configuration);
            RegisterShared(builder);
            RegisterModules(builder);
        }

        public static void UseRegisterMapHubSignalR(WebApplication app)
        {
            app.UseSharedModule();
            app.MapHub<NotificationHub>("/hubs/notifications").RequireAuthorization();
        }

        private static void RegisterShared(WebApplicationBuilder builder)
        {
            SharedService.WebApplicationBuilderSharedModule(builder);
            builder.Services.ServicesSharedModule(builder.Configuration);
        }

        private static void RegisterModules(WebApplicationBuilder builder)
        {
            builder.Services.RegisterColaboratorServices();
            builder.Services.RegisterManagementServices(builder.Configuration);
            builder.Services.RegisterLoginService();
        }
    }
}
