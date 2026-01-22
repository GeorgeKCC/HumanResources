using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Shared.RabbitMQ.Impl
{
    internal class RabbitMqConsumer(RabbitMQPersistentConnection connection) : IConsumerRabbitMQ
    {
        private readonly RabbitMQPersistentConnection _connection = connection;

        public async Task StartAsync(
            string queueName,
            Func<string, Task> onMessage,
            CancellationToken ct = default)
        {
            var conn = await _connection.GetConnectionAsync();
            var channel = await conn.CreateChannelAsync(cancellationToken: ct);

            _ = await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, args) =>
            {
                var json = Encoding.UTF8.GetString(args.Body.Span);

                await onMessage(json);

                await channel.BasicAckAsync(
                    args.DeliveryTag,
                    multiple: false);
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: ct);
        }
    }
}
