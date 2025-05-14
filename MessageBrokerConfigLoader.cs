using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

/// <summary>
/// This class is responsible for loading the RabbitMQ configuration from a YAML file.
/// It uses the YamlDotNet library to deserialize the YAML content into a RabbitMqConfig object.
/// </summary>
public static class MessageBrokerConfigLoader
{
    /// <summary>
    /// Loads the RabbitMQ configuration from a YAML file.
    /// The YAML file should contain a section named "rabbitmq" with the necessary configuration settings.
    /// </summary>
    /// <param name="filePath"> The path to the YAML file.</param>
    /// <returns>Returns a RabbitMqConfig object containing the configuration settings.</returns>
    /// <exception cref="Exception"> Thrown if the "rabbitmq" section is missing in the YAML configuration.</exception>
    /// <exception cref="FileNotFoundException"> Thrown if the specified YAML file does not exist.</exception>
    public static RabbitMqConfig LoadRabbitMqConfig(string filePath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
    
        var yaml = File.ReadAllText(filePath);
        var root = deserializer.Deserialize<Dictionary<string, RabbitMqConfig>>(yaml);
    
        if (root.TryGetValue("rabbitmq", out var rabbitMqConfig))
        {
            return rabbitMqConfig;
        }
        else
        {
            throw new Exception("Missing 'rabbitmq' section in the YAML configuration.");
        }
    }
}