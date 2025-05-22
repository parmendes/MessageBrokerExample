// This file defines the LightMeasurementProducer class, which is responsible for publishing light measurement events to RabbitMQ.
// It ensures the required exchange exists and provides a method to publish events. The class is designed for modularity and integration
// with the rest of the light measurement system, and is used by the LightMeasurementApi as the event producer.

using RabbitMQ.Client;
using System.Text.Json;

namespace StreetLightsApi.Services;

/// <summary>
/// Represents the producer of light measurement events.
/// </summary>
public class LightMeasurementProducer
{
    /// <summary>
    /// The name of the RabbitMQ exchange used for light measurements.
    /// </summary>
    public const string ExchangeName = "light.measured.exchange";
    /// <summary>
    /// The type of the RabbitMQ exchange used for light measurements.
    /// </summary>
    public const string ExchangeType = "direct";
    /// <summary>
    /// The name of the RabbitMQ queue used for light measurements.
    /// </summary>
    public const string QueueName = "light.measured.queue";
    /// <summary>
    /// Whether the exchange is durable.
    /// </summary>
    public const bool ExchangeDurable = true;
    /// <summary>
    /// Whether the queue is durable.
    /// </summary>
    public const bool QueueDurable = true;
    /// <summary>
    /// Whether the queue is auto-delete.
    /// </summary>
    public const bool QueueAutoDelete = false;

    // The RabbitMQ channel used for publishing messages
    private readonly IChannel _channel;
    // The exchange name for publishing
    private readonly string _exchangeName = "light.measured.exchange";
    // The routing key for publishing
    private readonly string _routingKey = "light.measured";

    /// <summary>
    /// The name of the channel for light measurements, explicitly referencing the channel class.
    /// </summary>
    public static readonly string Channel = LightMeasuredChannel.ChannelName;

    /// <summary>
    /// Initializes a new instance of the <see cref="LightMeasurementProducer"/> class and ensures the RabbitMQ exchange exists.
    /// </summary>
    public LightMeasurementProducer()
    {
        // Create a connection factory for RabbitMQ
        var factory = new ConnectionFactory() { HostName = "localhost" };
        // Establish a connection to RabbitMQ
        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        // Create a channel for communication
        var channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
        // Ensure the exchange exists (idempotent)
        channel.ExchangeDeclareAsync(exchange: LightMeasurementInfrastructure.ExchangeName, type: LightMeasurementInfrastructure.ExchangeType, durable: LightMeasurementInfrastructure.ExchangeDurable);
        _channel = channel;
    }

    /// <summary>
    /// Publishes the specified <see cref="LightMeasuredEvent"/>
    /// </summary>
    /// <param name="evt">The <see cref="LightMeasuredEvent"/> to publish</param>
    public async Task PublishLightMeasuredAsync(LightMeasuredEvent evt)
    {
        // Serialize the event to JSON and encode as bytes
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
        // Publish the message to RabbitMQ (implementation commented out)
        // await _channel.BasicPublishAsync<IBasicProperties>(
        //     exchange: string.Empty,
        //     routingKey: "light.measured",
        //     mandatory: false,
        //     basicProperties: null,
        //     body: body,
        //     cancellationToken: default
        // );
    }
}