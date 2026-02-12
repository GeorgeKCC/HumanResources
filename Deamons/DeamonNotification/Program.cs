using DeamonNotification.Features.BillingGenerate;
using DeamonNotification.Features.NotificationCollaborator;
using DeamonNotification.Features.ProcessGenerate;
using DeamonNotification.Workers;
using Shared.Context.HumanResource_Context;
using Shared.Quartz;
using Shared.Quartz.Contract;
using Shared.RabbitMQ;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServiceDatabaseHumanResourceContext(builder.Configuration);
builder.Services.AddServiceRabbitMQ(builder.Configuration);

builder.Services.AddScoped<INotificationCollaboratorPassword, NotificationCollaboratorPassword>();

builder.Services.AddScoped<IProcessGenerateCollaborator, ProcessGenerateCollaborator>();
builder.Services.AddScoped<IBillingGenerateCollaborator, BillingGenerateCollaborator>();

builder.Services.AddScoped<IScheduledTask, WorkerRecurrentProcessGenerate>();
builder.Services.AddScoped<IScheduledTask, WorkerRecurrentBillingGenerate>();

builder.Services.AddServiceQuartz(builder.Configuration);
builder.Services.AddHostedService<WorkerCollaboratorPassword>();

var host = builder.Build();
host.Run();
