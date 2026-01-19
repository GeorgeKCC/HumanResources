using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Shared.Securities.RabbitMQ.Impl
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
                Password = password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            try
            {
                if (_connection is { IsOpen: true })
                    return _connection;

                var retries = 3;

                while (retries-- > 0)
                {
                    try
                    {
                        _connection = await _factory.CreateConnectionAsync();
                        break;
                    }
                    catch (BrokerUnreachableException)
                    {
                        Console.WriteLine("RabbitMQ no disponible, reintentando...");
                    }
                }

                if (_connection == null)
                {
                    throw new Ex("No se pudo conectar a RabbitMQ");
                }

                return _connection;
            }
            catch(Ex ex)
            {
                throw new Ex(ex.Message);
            }
        }
    }
}