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

    private readonly IChannel _channel;
    private readonly string _exchangeName = "light.measured.exchange";
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
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
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
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
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