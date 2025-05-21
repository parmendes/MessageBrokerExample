using Neuroglia.AsyncApi.v3;

namespace StreetLightsApi.Services;

/// <summary>
/// Represents the API for light measurement events.
/// This API provides a unified interface for publishing and consuming light measurement events.
/// </summary>
[Neuroglia.AsyncApi.v2.AsyncApi(
    "Light Measurement API",
    "1.0.0",
    Description = "Unified API for light measurement events.",
    LicenseName = "Apache 2.0",
    LicenseUrl = "https://www.apache.org/licenses/LICENSE-2.0"
)]
[Neuroglia.AsyncApi.v3.AsyncApi(
    "Light Measurement API",
    "1.0.0",
    Description = "Unified API for light measurement events.",
    LicenseName = "Apache 2.0",
    LicenseUrl = "https://www.apache.org/licenses/LICENSE-2.0"
)]

[Neuroglia.AsyncApi.v3.Channel(
    "light.measured", // The name of the channel for light measurements
    Address = "#/channels/light.measured", // The address of the channel in the AsyncAPI document
    Servers = ["#/servers/mosquitto"], // The server(s) where the channel is available
    Description = "This channel is used to exchange messages about lightning measurements.",
    Bindings = "#/components/channelBindings/amqp" // The binding for the channel
    )]
[HttpServerBinding("http")] // The HTTP server binding for the channel
[Neuroglia.AsyncApi.v2.Tag("light", "A tag for light-related operations")] // A tag for light-related operations
[Neuroglia.AsyncApi.v2.Tag("measurement", "A tag for measurement-related operations")] // A tag for measurement-related operations
[Neuroglia.AsyncApi.v3.Tag(Name = "light", Description = "A tag for light-related operations")] // A tag for light-related operations
[Neuroglia.AsyncApi.v3.Tag(Name = "measurement", Description = "A tag for measurement-related operations")] // A tag for measurement-related operations
public class LightMeasurementApi
{
    private readonly LightMeasurementProducer _producer;
    private readonly LightMeasurementConsumer _consumer;

    /// <summary>
    /// Initializes a new instance of the <see cref="LightMeasurementApi"/> class.
    /// This constructor is used for dependency injection.
    /// </summary>
    /// <param name="producer"> The producer of light measurement events.</param>
    /// <param name="consumer"> The consumer of light measurement events.</param>
    public LightMeasurementApi(LightMeasurementProducer producer, LightMeasurementConsumer consumer)
    {
        _producer = producer;
        _consumer = consumer;
    }

    /// <summary>
    /// Publishes a light measured event to RabbitMQ.
    /// This method is used to notify remote consumers about environmental lighting conditions for a particular streetlight.
    /// </summary>
    /// <param name="evt">The light measured event to publish.</param>
    [Neuroglia.AsyncApi.v2.PublishOperation(
        typeof(LightMeasuredEvent),
        OperationId = "PublishLightMeasured",
        Summary = "Publishes a light measured event to RabbitMQ.",
        Description = "Notifies remote consumers about environmental lighting conditions for a particular streetlight."
    )]
    [Neuroglia.AsyncApi.v3.Operation(
        "publishLightMeasured",
        V3OperationAction.Send,
        "#/channels/light.measured",
        Description = "Publishes a light measured event to RabbitMQ.",
        Bindings = "#/components/operationBindings/amqp"
    )]
    [Neuroglia.AsyncApi.v2.Channel(LightMeasuredChannel.ChannelName)]
    [Neuroglia.AsyncApi.v3.Tag(Name = "measurement")]
    [Neuroglia.AsyncApi.v3.Tag(Name = "light")]
    public async Task PublishLightMeasuredAsync(LightMeasuredEvent evt)
        => await _producer.PublishLightMeasuredAsync(evt);

    /// <summary>
    /// Consumes a light measured event from RabbitMQ.
    /// This method is used to receive notifications about environmental lighting conditions for a particular streetlight.
    /// </summary>
    /// <param name="evt">The light measured event to consume.</param>
    [Neuroglia.AsyncApi.v2.SubscribeOperation(
        typeof(LightMeasuredEvent),
        OperationId = "ConsumeLightMeasured",
        Summary = "Consumes a light measured event from RabbitMQ.",
        Description = "Receives notifications about environmental lighting conditions for a particular streetlight."
    )]
    [Neuroglia.AsyncApi.v3.Operation(
        "consumeLightMeasured",
        V3OperationAction.Receive,
        "#/channels/light.measured",
        Description = "Consumes a light measured event from RabbitMQ.",
        Bindings = "#/components/operationBindings/amqp"
    )]
    [Neuroglia.AsyncApi.v2.Channel(LightMeasuredChannel.ChannelName)]
    [Neuroglia.AsyncApi.v3.Tag(Name = "measurement")]
    [Neuroglia.AsyncApi.v3.Tag(Name = "light")]
    public Task ConsumeLightMeasuredAsync(LightMeasuredEvent evt)
        => _consumer.ConsumeLightMeasuredAsync(evt);
}
