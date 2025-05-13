class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Loading RabbitMQ configuration...");
        var config = MessageBrokerConfigLoader.LoadRabbitMqConfig("rabbitmq-config.yaml");

        Console.WriteLine("Sending message to RabbitMQ...");
        var rabbitMqHelper = new RabbitMqHelper(config);
        await rabbitMqHelper.SendMessageAsync("Hello World!");

        Console.WriteLine("Generating AsyncAPI specification...");
        var asyncApiHelper = new AsyncApiHelper(config);
        asyncApiHelper.GenerateAsyncApiSpec();

        Console.WriteLine("Done.");
    }
}