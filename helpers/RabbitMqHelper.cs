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
    /// Initializes RabbitMQ by creating exchanges and queues based on the configuration.
    /// </summary>
    public async Task InitializeRabbitMqAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            UserName = _config.UserName,
            Password = _config.Password,
            Port = _config.Port,
            VirtualHost = _config.VirtualHost,
            RequestedHeartbeat = TimeSpan.FromSeconds(_config.RequestedHeartbeat),
            NetworkRecoveryInterval = TimeSpan.FromMilliseconds(_config.NetworkRecoveryInterval),
            Ssl = { Enabled = _config.UseSsl }
        };

        // var endpoints = new List<AmqpTcpEndpoint>
        // {
        //     new AmqpTcpEndpoint(_config.HostName)
        // };

        using (var connection = await factory.CreateConnectionAsync(/*endpoints*/))
        using (var channel = await connection.CreateChannelAsync())
        {
            // Declare exchanges explicitly listed in the configuration
            var declaredExchanges = new HashSet<string>();
            foreach (var exchange in _config.Exchanges)
            {
                await channel.ExchangeDeclareAsync(
                    exchange.Name,
                    exchange.Type,
                    exchange.Durable,
                    exchange.AutoDelete,
                    exchange.Arguments
                );
                declaredExchanges.Add(exchange.Name);
            }

            // Declare queues and bind them to exchanges
            foreach (var queue in _config.Queues)
            {
                // Ensure the exchange exists before binding
                if (!declaredExchanges.Contains(queue.Exchange))
                {
                    Console.WriteLine($"Exchange '{queue.Exchange}' not found in configuration. Declaring it as 'direct'.");
                    await channel.ExchangeDeclareAsync(
                        queue.Exchange,
                        "direct", // Default to "direct" if not explicitly configured
                        true,     // Durable
                        false,    // AutoDelete
                        null      // No additional arguments
                    );
                    declaredExchanges.Add(queue.Exchange);
                }

                await channel.QueueDeclareAsync(
                    queue.Name,
                    queue.Durable,
                    queue.Exclusive,
                    queue.AutoDelete,
                    ConvertArguments(queue.Arguments)
                );

                await channel.QueueBindAsync(queue.Name, queue.Exchange, queue.RoutingKey);
            }

            Console.WriteLine("RabbitMQ initialization completed.");
        }
    }

    /// <summary>
    /// Sends a message to the RabbitMQ queues based on the configuration.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public async Task SendMessageAsync(string message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            UserName = _config.UserName,
            Password = _config.Password,
            Port = _config.Port,
            VirtualHost = _config.VirtualHost,
            RequestedHeartbeat = TimeSpan.FromSeconds(_config.RequestedHeartbeat),
            NetworkRecoveryInterval = TimeSpan.FromMilliseconds(_config.NetworkRecoveryInterval),
            Ssl = { Enabled = _config.UseSsl }
        };

        using (var connection = await factory.CreateConnectionAsync())
        using (var channel = await connection.CreateChannelAsync())
        {
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

            // Send the message to each queue in the configuration
            foreach (var queue in _config.Queues)
            {
                byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(
                    queue.Exchange,
                    queue.RoutingKey,
                    false,
                    props,
                    messageBodyBytes
                );

                Console.WriteLine($" [x] Sent '{message}' to queue '{queue.Name}'");
            }
        }
    }

    private IDictionary<string, object?> ConvertArguments(Dictionary<string, object> arguments)
    {
        if (arguments == null)
            return new Dictionary<string, object?>();
    
        var convertedArguments = new Dictionary<string, object?>();
        foreach (var kvp in arguments)
        {
            // Convert specific arguments to the expected types
            if (kvp.Key == "x-message-ttl" && kvp.Value is string ttlString && int.TryParse(ttlString, out var ttl))
            {
                convertedArguments[kvp.Key] = ttl; // Convert to integer
            }
            else
            {
                convertedArguments[kvp.Key] = kvp.Value; // Keep as-is for other arguments
            }
        }
        return convertedArguments;
    }
}