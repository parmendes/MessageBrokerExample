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

// Save AsyncAPI document as a file
// var builderService = app.Services.GetRequiredService<IAsyncApiDocumentBuilder>();
// var asyncApiDocument = builderService.Build();

var provider = app.Services.GetRequiredService<IAsyncApiDocumentProvider>();

var asyncApiDocument = provider
    .GetDocumentAsync("default")     // <-- must pass the "default" group name
    .GetAwaiter()
    .GetResult();

var serializer = new SerializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();
var yamlContent = serializer.Serialize(asyncApiDocument);
File.WriteAllText("/asyncapi.yaml", yamlContent);

app.UseStaticFiles();

// Run
app.Run();