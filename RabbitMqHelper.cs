using RabbitMQ.Client;
using System.Text;

/// <summary>
/// This class provides methods to interact with RabbitMQ.
/// It includes methods to send messages to a RabbitMQ queue.
/// </summary>
public class RabbitMqHelper
{
    private readonly RabbitMqConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqHelper"/> class.
    /// </summary>
    /// <param name="config">The RabbitMQ configuration.</param>
    public RabbitMqHelper(RabbitMqConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Sends a message to the RabbitMQ queue.
    /// /summary>
    /// <param name="message">The message to send.</param>
    public async Task SendMessageAsync(string message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            UserName = _config.UserName,
            Password = _config.Password,
            Port = _config.Port
        };

        var endpoints = new List<AmqpTcpEndpoint>
        {
            new AmqpTcpEndpoint(_config.HostName)
        };

        using (var connection = await factory.CreateConnectionAsync(endpoints))
        using (var channel = await connection.CreateChannelAsync())
        {
            await channel.ExchangeDeclareAsync(_config.ExchangeName, ExchangeType.Direct);
            await channel.QueueDeclareAsync(_config.QueueName, false, false, false, null);
            await channel.QueueBindAsync(_config.QueueName, _config.ExchangeName, _config.RoutingKey, null);

            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);

            var props = new BasicProperties
            {
                ContentType = "text/plain",
                DeliveryMode = DeliveryModes.Persistent,
                Expiration = "36000000", // 10 hours in milliseconds
                Headers = new Dictionary<string, object?>
                {
                    { "latitude", 51.5252949 },
                    { "longitude", -0.0905493 },
                    { "x-delay", 1000 }
                }
            };

            await channel.BasicPublishAsync(_config.ExchangeName, _config.RoutingKey, false, props, messageBodyBytes);
            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}