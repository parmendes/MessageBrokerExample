using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;

/// <summary>
/// Represents the AMQP operation binding for AsyncAPI (version 0.2.0).
/// Implements the necessary interfaces for serialization and extensibility.
/// </summary>
public class AmqpOperationBindingModel : IOperationBinding, IBinding, IAsyncApiSerializable, IAsyncApiExtensible
{
    /// <summary>
    /// Indicates whether the message is mandatory.
    /// </summary>
    public bool Mandatory { get; set; }

    /// <summary>
    /// Indicates whether the message is immediate.
    /// </summary>
    public bool Immediate { get; set; }

    /// <summary>
    /// Specifies the AMQP binding version. Defaults to 0.2.0.
    /// </summary>
    public string BindingVersion { get; set; } = "0.2.0";

    /// <summary>
    /// Gets the binding key as defined by the AsyncAPI spec.
    /// </summary>
    public string BindingKey => "amqp";

    /// <summary>
    /// Serializes the binding for AsyncAPI v2 documents.
    /// </summary>
    /// <param name="writer">The AsyncAPI writer instance.</param>
    public void SerializeV2(IAsyncApiWriter writer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("mandatory");
        writer.WriteValue(Mandatory);

        writer.WritePropertyName("immediate");
        writer.WriteValue(Immediate);

        // Only include bindingVersion if it's not the default value
        if (BindingVersion != "0.2.0")
        {
            writer.WritePropertyName("bindingVersion");
            writer.WriteValue(BindingVersion);
        }

        writer.WriteExtensions(Extensions);
        writer.WriteEndObject();
    }

    /// <summary>
    /// Serializes the binding for AsyncAPI v3 documents (same as v2).
    /// </summary>
    /// <param name="writer">The AsyncAPI writer instance.</param>
    public void SerializeV3(IAsyncApiWriter writer)
    {
        SerializeV2(writer);
    }

    /// <summary>
    /// Gets or sets custom extensions for the binding.
    /// </summary>
    public IDictionary<string, IAsyncApiExtension> Extensions { get; set; }
        = new Dictionary<string, IAsyncApiExtension>();
}
