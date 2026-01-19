using Shared.RabbitMQ.Queued;

namespace DeamonNotification.Features
{
    public class NotificationCollaboratorPassword(ILogger<NotificationCollaboratorPassword> logger) : INotificationCollaboratorPassword
    {
        public async Task Handler(QueueCollaboratorPassword queue)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("NotificationCollaboratorPassword: Sending collaborator password notification to {Email} ({FullName})", queue.Email, queue.FullName);
            }
        }
    }
}
