using Shared.RabbitMQ.Queued;

namespace DeamonNotification.Features.NotificationCollaborator
{
    public interface INotificationCollaboratorPassword
    {
        Task Handler(QueueCollaboratorPassword queue);
    }
}
