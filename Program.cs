using RabbitMQ.Client;
using System.Text;
using LEGO.AsyncAPI.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.RepresentationModel;

class Program
{
    private static RabbitMqConfig config;

    static async Task Main(string[] args)
    {
        // Load RabbitMQ configuration from YAML
        LoadRabbitMqConfig();

        var factory = new ConnectionFactory()
        {
            HostName = config.HostName,
            UserName = config.UserName,
            Password = config.Password,
            Port = config.Port
        };

        var endpoints = new List<AmqpTcpEndpoint>
        {
            new AmqpTcpEndpoint(config.HostName)
        };

        using (var connection = await factory.CreateConnectionAsync(endpoints))
        using (var channel = await connection.CreateChannelAsync())
        {
            await channel.ExchangeDeclareAsync(config.ExchangeName, ExchangeType.Direct);
            await channel.QueueDeclareAsync(config.QueueName, false, false, false, null);
            await channel.QueueBindAsync(config.QueueName, config.ExchangeName, config.RoutingKey, null);

            string message = "Hello World!";
            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);
        
            var props = new BasicProperties
            {
                ContentType = "text/plain",
                DeliveryMode = DeliveryModes.Persistent,
                Expiration = "36000000", // 10 hours in milliseconds
                Headers = new Dictionary<string, object?>()
                {
                    { "latitude", 51.5252949 },
                    { "longitude", -0.0905493 },
                    { "x-delay", 1000 }
                }
            };

            await channel.BasicPublishAsync(config.ExchangeName, config.RoutingKey, false, props, messageBodyBytes);
            Console.WriteLine(" [x] Sent {0}", message);
        }

        var asyncApiDocument = new AsyncApiDocument
        {
            Info = new AsyncApiInfo
            {
                Title = "Message Broker Example (RabbitMQ)",
                Description = "This is an example of a message broker using RabbitMQ.",
                Version = "1.0.0"
            },
            Channels = new Dictionary<string, AsyncApiChannel>
            {
                {
                    config.QueueName, new AsyncApiChannel // Use the queue name from the configuration
                    {
                        Publish = new AsyncApiOperation
                        {
                            Message = new List<AsyncApiMessage>
                            {
                                new AsyncApiMessage
                                {
                                    ContentType = "text/plain",
                                    Payload = new AsyncApiSchema
                                    {
                                        Type = SchemaType.String
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var yaml = asyncApiDocument.SerializeAsYaml(LEGO.AsyncAPI.AsyncApiVersion.AsyncApi2_0);
        File.WriteAllText("asyncapispec/asyncapi.yaml", yaml);
        Console.WriteLine("AsyncAPI specification generated.");
    }

    private static void LoadRabbitMqConfig()
    {
        Console.WriteLine("LoadRabbitMqConfig START.");

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yaml = File.ReadAllText("rabbitmq-config.yaml");
        var root = deserializer.Deserialize<Dictionary<string, RabbitMqConfig>>(yaml);

        if (root.TryGetValue("rabbitmq", out var rabbitMqConfig))
        {
            config = rabbitMqConfig;
        }
        else
        {
            throw new Exception("Missing 'rabbitmq' section in the YAML configuration.");
        }

        Console.WriteLine("LoadRabbitMqConfig END.");
    }
}
