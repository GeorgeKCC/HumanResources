using Shared.RabbitMQ.Contract;

namespace Shared.RabbitMQ.Impl
{
    internal class PublishRabbitMQ(DatabaseContext databaseContext, PublisherRabbitMQ publisherRabbitMQ) : IPublishRabbitMQ
    {
        public async Task PublishAsync<T>(T message)
        {
            var queuedName = typeof(T).FullName!;
            var dataJson = JsonSerializer.Serialize(message);

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOn = DateTime.UtcNow,
                Type = queuedName,
                Payload = dataJson,
                Processed = false,
                ProcessedOn = null
            };

            databaseContext.OutboxMessages.Add(outboxMessage);
            await databaseContext.SaveChangesAsync();

            await publisherRabbitMQ.PublishRawAsync(queuedName, dataJson);
        }
    }
}
