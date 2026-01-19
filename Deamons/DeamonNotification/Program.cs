using DeamonNotification.Features;
using DeamonNotification.Workers;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.ServiceConnectionDatabase(builder.Configuration);
builder.Services.ConfigurationConsumerRabbitMQ(builder.Configuration);

builder.Services.AddScoped<INotificationCollaboratorPassword, NotificationCollaboratorPassword>();

builder.Services.AddHostedService<WorkerCollaboratorPassword>();

var host = builder.Build();
host.Run();
