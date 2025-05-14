using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;

/// <summary>
/// Represents the AMQP channel binding for AsyncAPI (version 0.2.0).
/// Implements the necessary interfaces for serialization and extensibility.
/// </summary>
public class AmqpChannelBindingModel : IChannelBinding, IBinding, IAsyncApiSerializable, IAsyncApiExtensible
{
    /// <summary>
    /// Defines how the data is transferred (e.g., routingKey).
    /// </summary>
    public required string Is { get; set; }

    /// <summary>
    /// Specifies the AMQP channel binding version. Defaults to 0.2.0.
    /// </summary>
    public string BindingVersion { get; set; } = "0.2.0";

    /// <summary>
    /// Exchange settings for the channel binding.
    /// </summary>
    public required ExchangeModel Exchange { get; set; }

    /// <summary>
    /// Gets the binding key as defined by the AsyncAPI spec.
    /// </summary>
    public string BindingKey => "amqp";

    /// <summary>
    /// Serializes the channel binding for AsyncAPI v2 documents.
    /// </summary>
    /// <param name="writer">The AsyncAPI writer instance.</param>
    public void SerializeV2(IAsyncApiWriter writer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("is");
        writer.WriteValue(Is);

        // Only include bindingVersion if it's not the default value
        if (BindingVersion != "0.2.0")
        {
            writer.WritePropertyName("bindingVersion");
            writer.WriteValue(BindingVersion);
        }

        writer.WritePropertyName("exchange");
        writer.WriteStartObject();
        writer.WritePropertyName("name");
        writer.WriteValue(Exchange.Name);
        writer.WritePropertyName("type");
        writer.WriteValue(Exchange.Type);
        writer.WritePropertyName("durable");
        writer.WriteValue(Exchange.Durable);
        writer.WritePropertyName("autoDelete");
        writer.WriteValue(Exchange.AutoDelete);
        writer.WriteEndObject();

        writer.WriteExtensions(Extensions);
        writer.WriteEndObject();
    }

    /// <summary>
    /// Serializes the channel binding for AsyncAPI v3 documents (same as v2).
    /// </summary>
    /// <param name="writer">The AsyncAPI writer instance.</param>
    public void SerializeV3(IAsyncApiWriter writer)
    {
        SerializeV2(writer);
    }

    /// <summary>
    /// Gets or sets custom extensions for the binding.
    /// </summary>
    public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } =
        new Dictionary<string, IAsyncApiExtension>();

    /// <summary>
    /// Defines the exchange structure for the AMQP channel binding.
    /// </summary>
    public class ExchangeModel
    {
        /// <summary>
        /// Exchange name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Exchange type (e.g., direct, topic).
        /// </summary>
        public required string Type { get; set; }

        /// <summary>
        /// Indicates whether the exchange is durable.
        /// </summary>
        public bool Durable { get; set; }

        /// <summary>
        /// Indicates whether the exchange is auto-deleted.
        /// </summary>
        public bool AutoDelete { get; set; }
    }
}