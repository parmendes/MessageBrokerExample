using Neuroglia.AsyncApi.v2;
using Neuroglia.AsyncApi.v3;

namespace StreetLightsApi.Services;

/// <summary>
/// Represents the consumer of light measurement events.
/// </summary>
[Neuroglia.AsyncApi.v2.Tag("light", "A tag for light-related operations")]
[Neuroglia.AsyncApi.v2.Tag("measurement", "A tag for measurement-related operations")]
[Neuroglia.AsyncApi.v3.Tag(Name = "light", Description = "A tag for light-related operations")]
[Neuroglia.AsyncApi.v3.Tag(Name = "measurement", Description = "A tag for measurement-related operations")]
public class LightMeasurementConsumer
{
    /// <summary>
    /// The name of the channel for light measurements, explicitly referencing the channel class.
    /// </summary>
    public static readonly string Channel = LightMeasuredChannel.ChannelName;

    /// <summary>
    /// Handles a received <see cref="LightMeasuredEvent"/>
    /// </summary>
    /// <param name="evt">The <see cref="LightMeasuredEvent"/> received</param>
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
        Description = "Consumes a light measured event from RabbitMQ."
    )]
    [Neuroglia.AsyncApi.v3.Tag(Name = "measurement")]
    [Neuroglia.AsyncApi.v3.Tag(Name = "light")]
    public Task ConsumeLightMeasuredAsync(LightMeasuredEvent evt)
    {
        // Handle the event (implementation omitted)
        return Task.CompletedTask;
    }
}
