using DeamonNotification.Features;
using DeamonNotification.Workers;
using Shared.Context;
using Shared.RabbitMQ;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServiceDatabaseContext(builder.Configuration);
builder.Services.AddServiceRabbitMQ(builder.Configuration);

builder.Services.AddScoped<INotificationCollaboratorPassword, NotificationCollaboratorPassword>();

builder.Services.AddHostedService<WorkerCollaboratorPassword>();

var host = builder.Build();
host.Run();
