using System.Text;
using RabbitMQ.Client;
using RabbitMQAsyncAPI.Configurations;

namespace RabbitMQAsyncAPI.Services
{
    /// <summary>
    /// A service for producing messages to RabbitMQ.
    /// </summary>
    public class RabbitMqProducerService
    {
        private readonly IConnection _connection;
        private readonly RabbitMqOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqProducerService"/> class.
        /// </summary>
        /// <param name="config">The configuration object.</param>
        public RabbitMqProducerService(IConfiguration config)
        {
            _options = config
                .GetSection("RabbitMQ")
                .Get<RabbitMqOptions>()
                ?? throw new InvalidOperationException("RabbitMQ configuration section missing");

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password
            };

            // 
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Publishes a JSON message to the configured exchange and routing key, using the async API.
        /// </summary>
        public async Task PublishAsync(string message, CancellationToken cancellationToken = default)
        {
            // Validate the message
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or empty", nameof(message));
            }

            // Create a channel
            await using var channel = await _connection
                .CreateChannelAsync()
                .ConfigureAwait(false);

            // Declare the exchange
            await channel.ExchangeDeclareAsync(
                exchange: _options.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null)
            .ConfigureAwait(false);

            // Create a new BasicProperties object
            var props = new BasicProperties 
            {
                Headers = null, // No headers for this message
                MessageId = Guid.NewGuid().ToString(), // Unique message ID
                AppId = "RabbitMQAsyncAPI", // Application ID
                UserId = _options.UserName, // User ID from the configuration
                Priority = 0, // Default priority
                CorrelationId = Guid.NewGuid().ToString(), // Unique correlation ID
                ContentEncoding = "UTF-8", // Content encoding
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent, // Make the message persistent
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            var body = Encoding.UTF8.GetBytes(message);

            // Publish the message to the exchange with the specified routing key
            await channel.BasicPublishAsync(
                exchange: _options.Exchange,
                routingKey: _options.RoutingKey,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken)
            .ConfigureAwait(false);
        }
    }
}
