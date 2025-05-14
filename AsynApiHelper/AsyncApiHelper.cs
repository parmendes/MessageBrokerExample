using System.Text.Json;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;

public class AsyncApiHelper
{
    private readonly RabbitMqConfig _config;

    public AsyncApiHelper(RabbitMqConfig config)
    {
        _config = config;
    }

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
            Servers = new Dictionary<string, AsyncApiServer>
            {
                {
                    "rabbitmq", new AsyncApiServer
                    {
                        Url = $"amqp://{_config.HostName}:{_config.Port}",
                        Protocol = "amqp",
                        Description = "RabbitMQ server"
                    }
                }
            },
            Components = new AsyncApiComponents
            {
                SecuritySchemes = new Dictionary<string, AsyncApiSecurityScheme>
                {
                    {
                        "userPassword", new AsyncApiSecurityScheme
                        {
                            Type = SecuritySchemeType.UserPassword,
                            Description = "RabbitMQ username and password authentication"
                        }
                    }
                },
                Schemas = new Dictionary<string, AsyncApiSchema>
                {
                    {
                        "MessagePayload", new AsyncApiSchema
                        {
                            Type = SchemaType.Object,
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                { "messageId", new AsyncApiSchema { Type = SchemaType.String } },
                                { "timestamp", new AsyncApiSchema { Type = SchemaType.String, Format = "date-time" } },
                                { "data", new AsyncApiSchema { Type = SchemaType.Object } }
                            }
                        }
                    }
                }
            },
            Channels = new Dictionary<string, AsyncApiChannel>()
        };

        // Dynamically add channels for each queue
        foreach (var queue in _config.Queues)
        {
            asyncApiDocument.Channels.Add(queue.Name, new AsyncApiChannel
            {
                Description = $"Queue for {queue.Name}. Retry configuration: " +
                    $"MaxRetries={_config.RetryConfig.MaxRetries}, " +
                    $"RetryInterval={_config.RetryConfig.RetryInterval}ms, " +
                    $"DeadLetterExchange={_config.RetryConfig.DeadLetterExchange}, " +
                    $"DeadLetterQueue={_config.RetryConfig.DeadLetterQueue}.",
                Publish = new AsyncApiOperation
                {
                    Summary = $"Publish messages to the {queue.Name} queue",
                    OperationId = $"publishTo{queue.Name}",
                    Message = new List<AsyncApiMessage>
                    {
                        new AsyncApiMessage
                        {
                            Name = "Message",
                            Title = "Message",
                            ContentType = "application/json",
                            Payload = GeneratePayloadSchema(queue),
                            Headers = new AsyncApiSchema
                            {
                                Type = SchemaType.Object,
                                Properties = new Dictionary<string, AsyncApiSchema>
                                {
                                    { "correlationId", new AsyncApiSchema { Type = SchemaType.String } },
                                    { "description", new AsyncApiSchema { Type = SchemaType.String } }
                                }
                            },
                            CorrelationId = new AsyncApiCorrelationId
                            {
                                Location = "$message.header#/correlationId",
                                Description = "Correlation ID for tracking"
                            },
                            Examples = new List<AsyncApiMessageExample>
                            {
                                new() {
                                    Name    = "ExampleMessage",
                                    Summary = "A minimal valid payload",
                                    Payload = new AsyncApiAny(
                                        JsonSerializer.SerializeToNode(new
                                        {
                                            messageId = "12345",
                                            timestamp = DateTime.UtcNow.ToString("o"),
                                            data      = new { key = "value" }
                                        })!
                                    )
                                }
                            }
                        },
                    },
                    Bindings = new AsyncApiBindings<IOperationBinding>
                    {
                        ["amqp"] = new AmqpOperationBindingModel
                        {
                            Mandatory = false,
                            Immediate = false
                        }
                    },
                },
                Bindings = new AsyncApiBindings<IChannelBinding>
                {
                    ["amqp"] = new AmqpChannelBindingModel
                    {
                        Is = "routingKey",
                        Exchange = new AmqpChannelBindingModel.ExchangeModel
                        {
                            Name = queue.Exchange,
                            Type = "direct",
                            Durable = queue.Durable,
                            AutoDelete = queue.AutoDelete
                        }
                    }
                }
            });
        }

        // Serialize the AsyncAPI document to YAML
        var yaml = asyncApiDocument.SerializeAsYaml(LEGO.AsyncAPI.AsyncApiVersion.AsyncApi2_0);
        File.WriteAllText("asyncapispec/asyncapi.yaml", yaml);

        Console.WriteLine("AsyncAPI specification generated.");
    }

    private AsyncApiSchema GeneratePayloadSchema(RabbitMqQueueConfig queue)
    {
        return new AsyncApiSchema
        {
            Type = SchemaType.Object,
            Description = $"Message payload for the {queue.Name} queue",
            Properties = new Dictionary<string, AsyncApiSchema>
            {
                { "messageId", new AsyncApiSchema { Type = SchemaType.String, Description = "Unique identifier for the message" } },
                { "timestamp", new AsyncApiSchema { Type = SchemaType.String, Format = "date-time", Description = "Time the message was created" } },
                { "data", new AsyncApiSchema { Type = SchemaType.Object, Description = "Payload data" } }
            },
            Required = new HashSet<string> { "messageId", "timestamp" } // Define required fields
        };
    }
}