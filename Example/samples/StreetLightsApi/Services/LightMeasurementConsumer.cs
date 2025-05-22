// This class is responsible for consuming light measurement events from RabbitMQ.
// It sets up the necessary exchange, queue, and binding using RabbitMQ.Client async APIs.
// The consumer listens for messages on the configured queue and provides a handler for processing received events.

using RabbitMQ.Client;

namespace StreetLightsApi.Services;

/// <summary>
/// Represents the consumer of light measurement events.
/// </summary>
// The AsyncAPI channel for light measurements
public class LightMeasurementConsumer
{
    // Exchange name used for light measurement events
    private readonly string exchangeName = "light.measured.exchange";
    // Queue name used for consuming light measurement events
    private readonly string queueName = "light.measured.queue";
    // Routing key for binding the queue to the exchange
    private readonly string routingKey = "light.measured";

    /// <summary>
    /// Initializes a new instance of the <see cref="LightMeasurementConsumer"/> class and sets up RabbitMQ exchange, queue, and binding.
    /// </summary>
    public LightMeasurementConsumer()
    {
        // Create a connection factory for RabbitMQ
        var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "localhost" };
        // Establish a connection to RabbitMQ
        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        // Create a channel for communication
        var channel = connection.CreateChannelAsync().GetAwaiter().GetResult();

        // Declare the exchange for light measurement events
        channel.ExchangeDeclareAsync(LightMeasurementInfrastructure.ExchangeName, LightMeasurementInfrastructure.ExchangeType);
        // Declare the queue for consuming light measurement events
        channel.QueueDeclareAsync(LightMeasurementInfrastructure.QueueName, LightMeasurementInfrastructure.QueueDurable, false, LightMeasurementInfrastructure.QueueAutoDelete, null);
        // Bind the queue to the exchange with the routing key
        channel.QueueBindAsync(LightMeasurementInfrastructure.QueueName, LightMeasurementInfrastructure.ExchangeName, LightMeasurementInfrastructure.RoutingKey, null);
    }

    /// <summary>
    /// The name of the channel for light measurements, explicitly referencing the channel class.
    /// </summary>
    public static readonly string Channel = LightMeasuredChannel.ChannelName;

    /// <summary>
    /// Handles a received <see cref="LightMeasuredEvent"/>
    /// </summary>
    /// <param name="evt">The <see cref="LightMeasuredEvent"/> received</param>
    public Task ConsumeLightMeasuredAsync(LightMeasuredEvent evt)
    {
        // Handle the event (implementation omitted)
        return Task.CompletedTask;
    }
}
