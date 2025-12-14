using RabbitMQ.Client;

namespace Shared.RabbitMQ.Impl
{
    internal class PublisherRabbitMQ(RabbitMQPersistentConnection connection)
    {
        private readonly RabbitMQPersistentConnection _connection = connection;

        public async Task PublishRawAsync(string queuedName, string rawJson)
        {
            var conn = await _connection.GetConnectionAsync();

            await using var channel = await conn.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "event.humanresource",
                type: ExchangeType.Direct,
                durable: true
            );

            await channel.QueueDeclareAsync(
               queue: queuedName,
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null
            );

            await channel.QueueBindAsync(
                queue: queuedName,
                exchange: "event.humanresource",
                routingKey: "humanresourcekey"
            );

            var body = Encoding.UTF8.GetBytes(rawJson);

            await channel.BasicPublishAsync(
                exchange: "event.humanresource",
                routingKey: "humanresourcekey",
                mandatory: false,
                body: body
            );
        }
    }
}
