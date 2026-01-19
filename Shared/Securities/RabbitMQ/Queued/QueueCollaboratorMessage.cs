namespace Shared.Securities.RabbitMQ.Queued
{
    public record QueueCollaboratorMessage(int CollaboratorId,string Subject, string Message);
}
