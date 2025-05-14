/// <summary>
/// This class represents the configuration settings for RabbitMQ.
/// It includes properties for the hostname, username, password, port,
/// </summary>
public class RabbitMqConfig
{
    public required string HostName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public bool UseSsl { get; set; } = false;
    public int ConnectionTimeout { get; set; } = 30000;
    public int RequestedHeartbeat { get; set; } = 60;
    public int NetworkRecoveryInterval { get; set; } = 5000;

    public List<RabbitMqQueueConfig> Queues { get; set; } = new List<RabbitMqQueueConfig>();
    public List<RabbitMqExchangeConfig> Exchanges { get; set; } = new List<RabbitMqExchangeConfig>();
    public required RabbitMqRetryConfig RetryConfig { get; set; }
    public required RabbitMqMonitoringConfig MonitoringConfig { get; set; }
}

public class RabbitMqQueueConfig
{
    public required string Name { get; set; }
    public required string Exchange { get; set; }
    public required string RoutingKey { get; set; }
    public bool Durable { get; set; } = true; // Whether the queue survives broker restarts
    public bool Exclusive { get; set; } = false; // Whether the queue is exclusive to the connection
    public bool AutoDelete { get; set; } = false; // Whether the queue is deleted when no consumers are connected
    public Dictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>(); // Additional arguments for the queue
}

public class RabbitMqExchangeConfig
{
    public required string Name { get; set; }
    public string Type { get; set; } = "direct"; // Exchange type (direct, topic, fanout, headers)
    public bool Durable { get; set; } = true; // Whether the exchange survives broker restarts
    public bool AutoDelete { get; set; } = false; // Whether the exchange is deleted when no queues are bound to it
    public Dictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>(); // Additional arguments for the exchange
}

public class RabbitMqConsumerConfig
{
    public required string QueueName { get; set; } // The queue to consume from
    public bool AutoAck { get; set; } = false; // Whether to automatically acknowledge messages
    public int PrefetchCount { get; set; } = 1; // Number of messages to prefetch
    public required string ConsumerTag { get; set; } // Custom consumer tag
}

public class RabbitMqPublisherConfig
{
    public required string Exchange { get; set; } // The exchange to publish to
    public required string RoutingKey { get; set; } // The routing key for the message
    public string ContentType { get; set; } = "application/json"; // Content type of the message
    public bool Persistent { get; set; } = true; // Whether the message is persistent
    public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>(); // Custom headers for the message
}

public class RabbitMqRetryConfig
{
    public int MaxRetries { get; set; } = 3; // Maximum number of retries
    public int RetryInterval { get; set; } = 5000; // Interval between retries (in milliseconds)
    public required string DeadLetterExchange { get; set; } // Exchange for dead-letter messages
    public required string DeadLetterQueue { get; set; } // Queue for dead-letter messages
}

public class RabbitMqMonitoringConfig
{
    public bool EnableMetrics { get; set; } = false; // Whether to enable metrics collection
    public required string MetricsEndpoint { get; set; } // Endpoint for sending metrics
}