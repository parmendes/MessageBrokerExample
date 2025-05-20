using Neuroglia.AsyncApi.v2;
using Neuroglia.AsyncApi.v3;

namespace StreetLightsApi.Services;

/// <summary>
/// Defines the AsyncAPI channel for light measurements.
/// </summary>
// [Neuroglia.AsyncApi.v2.Channel("light.measured")]
// [Neuroglia.AsyncApi.v3.Channel("light.measured", Description = "This channel is used to exchange messages about lightning measurements.")]
public class LightMeasuredChannel 
{
    /// <summary>
    /// The name of the channel for light measurements.
    /// </summary>
    public const string ChannelName = "light.measured";
}
