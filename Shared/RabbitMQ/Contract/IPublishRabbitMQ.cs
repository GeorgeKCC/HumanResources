namespace Shared.RabbitMQ.Contract
{
    public interface IPublishRabbitMQ
    {
        Task PublishAsync<T>(T message);
    }
}
