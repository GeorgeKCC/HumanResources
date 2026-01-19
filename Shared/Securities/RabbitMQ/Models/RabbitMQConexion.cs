using System.Text.Json.Serialization;

namespace Shared.Securities.RabbitMQ.Models
{
    internal class RabbitMQConexion
    {
        [JsonPropertyName("Host")]
        public required string Host { get; set; }
        [JsonPropertyName("UserName")]
        public required string UserName { get; set; }
        [JsonPropertyName("Password")]
        public required string Password { get; set; }
    }
}
