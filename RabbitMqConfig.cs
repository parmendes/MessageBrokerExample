/// <summary>
/// This class represents the configuration settings for RabbitMQ.
/// It includes properties for the hostname, username, password, port,
/// </summary>
public class RabbitMqConfig
{
    public required string HostName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public int Port { get; set; }
    public required string ExchangeName { get; set; }
    public required string QueueName { get; set; }
    public required string RoutingKey { get; set; }
}