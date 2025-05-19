using RabbitMQ.Client;
using System.Text.Json;
using Neuroglia.AsyncApi.v2;
using Neuroglia.AsyncApi.v3;

namespace StreetLightsApi.Services;

/// <summary>
/// Represents the producer of light measurement events.
/// </summary>
[Server("mosquitto", "amqp://localhost:5672", AsyncApiProtocol.Amqp, Description = "RabbitMQ server for light measurement events")]
[Neuroglia.AsyncApi.v2.Channel("light.measured")]
[Neuroglia.AsyncApi.v2.Tag("light", "A tag for light-related operations")]
[Neuroglia.AsyncApi.v2.Tag("measurement", "A tag for measurement-related operations")]
public class LightMeasurementProducer
{
    private readonly IChannel _channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="LightMeasurementProducer"/> class.
    /// Creates a connection to RabbitMQ.
    /// </summary>
    public LightMeasurementProducer()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Publishes the specified <see cref="LightMeasuredEvent"/>
    /// </summary>
    /// <param name="evt">The <see cref="LightMeasuredEvent"/> to publish</param>
    [PublishOperation(
        OperationId = "PublishLightMeasured",
        Summary = "Publishes a light measured event to RabbitMQ.",
        Description = "Notifies remote consumers about environmental lighting conditions for a particular streetlight."
    )]
    [Neuroglia.AsyncApi.v3.Operation(
        "publishLightMeasured",
        V3OperationAction.Send,
        "#/channels/light.measured",
        Description = "Publishes a light measured event to RabbitMQ.")]
    [Neuroglia.AsyncApi.v3.Tag(Reference = "#/components/tags/measurement")]
    [Neuroglia.AsyncApi.v3.Tag(Reference = "#/components/tags/light")]
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