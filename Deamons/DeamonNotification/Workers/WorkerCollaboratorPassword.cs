using DeamonNotification.Features;
using Shared.RabbitMQ.Contract;
using Shared.RabbitMQ.Queued;
using System.Text.Json;

namespace DeamonNotification.Workers
{
    public class WorkerCollaboratorPassword(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WorkerCollaboratorPassword>>();
            var _consumer = scope.ServiceProvider.GetRequiredService<IConsumerRabbitMQ>();
            var notificationCollaboratorPassword = scope.ServiceProvider.GetRequiredService<INotificationCollaboratorPassword>();
            var queueName = typeof(QueueCollaboratorPassword).FullName ?? nameof(QueueCollaboratorPassword);

            return _consumer.StartAsync(
                queueName: queueName,
                async json =>
                {
                    if (logger.IsEnabled(LogLevel.Information))
                    {
                        logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    }
                    var evt = JsonSerializer.Deserialize<QueueCollaboratorPassword>(json)!;
                    await notificationCollaboratorPassword.Handler(evt);
                },
                stoppingToken);
        }
    }
}
