using System.Numerics;
// Copyright © 2021-Present Neuroglia SRL. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace StreetLightsApi.Messages;

/// <summary>
/// Represents an event fired whenever light has been measured
/// </summary>
[Neuroglia.AsyncApi.v2.Tag("light", "A tag for light-related messages"), Neuroglia.AsyncApi.v2.Tag("measurement", "A tag for measurement-related messages")]
[Neuroglia.AsyncApi.v2.Message(Name = "LightMeasured", Description = "A message used to measure light")]
[Neuroglia.AsyncApi.v3.Tag(Name = "light", Description = "A tag for light-related messages"), Neuroglia.AsyncApi.v3.Tag(Name = "measurement", Description = "A tag for measurement-related messages")]
[Neuroglia.AsyncApi.v3.Message(Name = "LightMeasured", Description = "A message used to measure light")]
public class LightMeasuredEvent
{

    /// <summary>
    /// Gets/sets the id of the measured light
    /// </summary>
    [Required, Description("The id of the measured light ")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets/sets the type of the measured light
    /// </summary>
    [Required, Description("The type of the measured light")]
    public StreetLightType Type { get; set; }

    /// <summary>
    /// Gets/sets the specified light's lumens measurement
    /// </summary>
    [Required, Description("The specified light's lumens measurement")]
    public int Lumens { get; set; }

    /// <summary>
    /// Gets/sets the date and time at which the event has been created
    /// </summary>
    [Required, Description("The date and time at which the event has been created")]
    public DateTime SentAt { get; set; }

    /// <summary>
    /// Gets/sets the event's metadata
    /// </summary>
    [Description("The event's metadata")]
    public IDictionary<string, string>? Metadata { get; set; }

}
