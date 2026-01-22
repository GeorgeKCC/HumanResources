using Shared.RabbitMQ.Queued;

namespace DeamonNotification.Features
{
    public interface INotificationCollaboratorPassword
    {
        Task Handler(QueueCollaboratorPassword queue);
    }
}
