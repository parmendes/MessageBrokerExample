using Neuroglia.AsyncApi.v2;
using Neuroglia.AsyncApi.v3;

namespace StreetLightsApi.Services;

/// <summary>
/// Represents the consumer of light measurement events.
/// </summary>

// The AsyncAPI channel for light measurements
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
    public Task ConsumeLightMeasuredAsync(LightMeasuredEvent evt)
    {
        // Handle the event (implementation omitted)
        return Task.CompletedTask;
    }
}
