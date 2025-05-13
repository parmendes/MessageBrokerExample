using LEGO.AsyncAPI.Models;

/// <summary>
/// This class is responsible for generating the AsyncAPI specification.
/// It uses the RabbitMQ configuration to define the channels and messages.
/// </summary>
public class AsyncApiHelper
{
    private readonly RabbitMqConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncApiHelper"/> class.
    /// </summary>
    /// <param name="config">The RabbitMQ configuration.</param>
    public AsyncApiHelper(RabbitMqConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Generates the AsyncAPI specification and saves it to a YAML file.
    /// </summary>
    public void GenerateAsyncApiSpec()
    {
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
                    _config.QueueName, new AsyncApiChannel
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

        // Serialize the AsyncAPI document to YAML format
        // and save it to a file
        var yaml = asyncApiDocument.SerializeAsYaml(LEGO.AsyncAPI.AsyncApiVersion.AsyncApi2_0);
        File.WriteAllText("asyncapispec/asyncapi.yaml", yaml);
        Console.WriteLine("AsyncAPI specification generated.");
    }
}