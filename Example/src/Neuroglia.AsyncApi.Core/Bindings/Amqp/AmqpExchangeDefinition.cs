﻿// Copyright © 2021-Present Neuroglia SRL. All rights reserved.
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

namespace Neuroglia.AsyncApi.Bindings.Amqp;

/// <summary>
/// Represents the object used to configure an AMQP routing key based channel
/// </summary>
[DataContract]
public record AmqpExchangeDefinition
{

    /// <summary>
    /// Gets the default virtual host for AMQP exchanges
    /// </summary>
    public const string DefaultVirtualHost = "/";

    /// <summary>
    /// Gets/sets the name of the exchange. It MUST NOT exceed 255 characters long.
    /// </summary>
    [DataMember(Order = 1, Name = "name"), JsonPropertyOrder(1), JsonPropertyName("name"), YamlMember(Order = 1, Alias = "name")]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets/sets the <see cref="AmqpExchangeDefinition"/>'s type
    /// </summary>
    [DataMember(Order = 2, Name = "type"), JsonPropertyOrder(2), JsonPropertyName("type"), YamlMember(Order = 2, Alias = "type")]
    public virtual AmqpExchangeType Type { get; set; }

    /// <summary>
    /// Gets/sets a boolean indicating whether the exchange should survive broker restarts or not.
    /// </summary>
    [DataMember(Order = 3, Name = "durable"), JsonPropertyOrder(3), JsonPropertyName("durable"), YamlMember(Order = 3, Alias = "durable")]
    public virtual bool Durable { get; set; }

    /// <summary>
    /// Gets/sets a boolean indicating whether the exchange should be deleted when the last queue is unbound from it.
    /// </summary>
    [DataMember(Order = 4, Name = "autoDelete"), JsonPropertyOrder(4), JsonPropertyName("autoDelete"), YamlMember(Order = 4, Alias = "autoDelete")]
    public virtual bool AutoDelete { get; set; }

    /// <summary>
    /// Gets/sets the virtual host of the exchange. Defaults to '/'.
    /// </summary>
    [DataMember(Order = 5, Name = "vhost"), JsonPropertyOrder(5), JsonPropertyName("vhost"), YamlMember(Order = 5, Alias = "vhost")]
    public virtual string VirtualHost { get; set; } = DefaultVirtualHost;

    /// <inheritdoc/>
    public override string? ToString() => Name;

}
