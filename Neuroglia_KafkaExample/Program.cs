using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neuroglia.AsyncApi.Annotations;
using Neuroglia.AsyncApi.Models;
using Neuroglia.AsyncApi.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Config Kafka settings
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

// Register Kafka producer and consumer services
builder.Services.AddSingleton<KafkaProducerService>();
builder.Services.AddHostedService<KafkaConsumerService>();

// Add AsyncAPI generation services
builder.Services.AddAsyncApi();

// Build app
var app = builder.Build();

app.UseAsyncApi();     // Serve the AsyncAPI document JSON
app.UseAsyncApiUI();   // Serve the AsyncAPI web UI

// Define HTTP endpoint to send Kafka message
app.MapPost("/produce", async (KafkaProducerService producer, string message) =>
{
    await producer.ProduceAsync(message);
    return Results.Ok(new { status = "Message sent to Kafka", message });
});

app.Run();


// ----------- Kafka settings --------------

public record KafkaSettings
{
    public string BootstrapServers { get; init; } = "localhost:9092";
    public string Topic { get; init; } = "test-topic";
    public string GroupId { get; init; } = "test-group";
}

// ----------- Kafka message model -----------

[AsyncApi]
[AsyncApiMessage("KafkaMessage", Summary = "Kafka message", PayloadType = typeof(string))]
public class KafkaMessage { }

// ----------- Producer service -------------

[AsyncApi]
public class KafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducerService(Microsoft.Extensions.Options.IOptions<KafkaSettings> options)
    {
        var config = new ProducerConfig { BootstrapServers = options.Value.BootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _topic = options.Value.Topic;
    }

    [AsyncApiChannel("kafka/messages")]
    [AsyncApiOperation(OperationType.Publish, Summary = "Send message to Kafka")]
    [AsyncApiMessage(typeof(KafkaMessage), Name = "KafkaMessage")]
    public async Task ProduceAsync(string message)
    {
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
    }
}

// ----------- Consumer service -------------

[AsyncApi]
[AsyncApiChannel("kafka/messages")]
[AsyncApiOperation(OperationType.Subscribe, Summary = "Receive message from Kafka")]
[AsyncApiMessage(typeof(KafkaMessage), Name = "KafkaMessage")]
public class KafkaConsumerService : BackgroundService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly KafkaSettings _settings;

    public KafkaConsumerService(Microsoft.Extensions.Options.IOptions<KafkaSettings> options, ILogger<KafkaConsumerService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            GroupId = _settings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_settings.Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var cr = consumer.Consume(stoppingToken);
                _logger.LogInformation($"Kafka message received: {cr.Message.Value}");
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kafka consumer error");
            }
        }
    }
}
