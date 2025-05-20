using Neuroglia.AsyncApi.v2;
using Neuroglia.AsyncApi.v3;
using RabbitMQ.Client;

namespace StreetLightsApi.Services;

/// <summary>
/// Represents the consumer of light measurement events.
/// </summary>

// The AsyncAPI channel for light measurements
public class LightMeasurementConsumer
{
    private readonly string exchangeName = "light.measured.exchange";
    private readonly string queueName = "light.measured.queue";
    private readonly string routingKey = "light.measured";

    /// <summary>
    /// Initializes a new instance of the <see cref="LightMeasurementConsumer"/> class and sets up RabbitMQ exchange, queue, and binding.
    /// </summary>
    public LightMeasurementConsumer()
    {
        var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "localhost" };
        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        var channel = connection.CreateChannelAsync().GetAwaiter().GetResult();

        channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
        channel.QueueDeclareAsync(queueName, false, false, false, null);
        channel.QueueBindAsync(queueName, exchangeName, routingKey, null);
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
