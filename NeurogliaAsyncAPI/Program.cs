using Neuroglia.AsyncApi;
using RabbitMQAsyncAPI.Configurations;
using RabbitMQAsyncAPI.Endpoints;
using Neuroglia.AsyncApi.Generation;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Neuroglia.AsyncApi.FluentBuilders;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddAsyncApiConfiguration();

// Build the app
var app = builder.Build();

// Use Swagger
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API v1"));

// Map endpoints
app.MapWeatherEndpoints();

app.UseRouting();

// Map Async API
app.MapAsyncApiDocuments();

// Generate AsyncAPI document
// This is a blocking call, so it should be done after the app is built
// and before the app is run.
// This is a simple example, in a real-world application, you might want to
// generate the document in a background service or a separate process.
// This is just to demonstrate the generation of the AsyncAPI document
var provider = app.Services.GetRequiredService<IAsyncApiDocumentProvider>();

var asyncApiDocument = provider
    .GetDocumentAsync("default")
    .GetAwaiter()
    .GetResult();

Console.WriteLine("[AsyncAPI] Document generated: "+ asyncApiDocument?.Title);

// Serialize the AsyncAPI document to YAML
// using YamlDotNet.Serialization;
var serializer = new SerializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();

var yamlContent = serializer.Serialize(asyncApiDocument);

Directory.CreateDirectory("AsyncAPI");
File.WriteAllText("AsyncAPI/asyncapi.yaml", yamlContent);


app.UseStaticFiles();

// Run
app.Run();