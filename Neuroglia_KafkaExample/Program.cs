using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Neuroglia_KafkaExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Kafka settings
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

// Register Kafka producer and consumer
builder.Services.AddSingleton<KafkaProducerService>();
builder.Services.AddHostedService<KafkaConsumerService>();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/produce", async (KafkaProducerService producer, string message) =>
{
    await producer.ProduceAsync(message);
    return Results.Ok(new { status = "Message sent to Kafka", message });
});

app.Run();
