namespace Shared.RabbitMQ
{
    public static class UseServiceRabbitMQ
    {
        public static IServiceCollection AddServiceRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQ = configuration.GetSection("MessageBroker").Get<RabbitMQConexion>() ?? throw new Ex("Not implemented configuration rabbitmq");
            services.AddSingleton(new RabbitMQPersistentConnection(
                hostName: rabbitMQ.Host,
                userName: rabbitMQ.UserName,
                password: rabbitMQ.Password
            ));

            services.AddSingleton<PublisherRabbitMQ>();
            services.AddScoped<IPublishRabbitMQ, RabbitMqPublisher>();
            services.AddScoped<IConsumerRabbitMQ, RabbitMqConsumer>();

            return services;
        }
    }
}
