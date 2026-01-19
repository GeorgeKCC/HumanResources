namespace Shared.Securities.RabbitMQ.Contract
{
    public interface IPublishRabbitMQ
    {
        Task PublishAsync<T>(T message);
    }
}
