namespace Shared.RabbitMQ.Contract
{
    public interface IConsumerRabbitMQ
    {
        Task StartAsync(string queueName, Func<string, Task> onMessage, CancellationToken cancellationToken = default);
    }
}
