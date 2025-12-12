using RabbitMQ.Client;
using Shared.RabbitMQ.Contract;

namespace Shared.RabbitMQ.Impl
{
    public class RabbitMQPersistentConnection
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;

        public RabbitMQPersistentConnection(string hostName, string userName, string password)
        {
            _factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            if (_connection is { IsOpen: true })
                return _connection;

            _connection = await _factory.CreateConnectionAsync();
            return _connection;
        }
    }
}