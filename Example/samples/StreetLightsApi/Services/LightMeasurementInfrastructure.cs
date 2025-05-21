namespace StreetLightsApi.Services
{
    /// <summary>
    /// Centralizes RabbitMQ infrastructure settings for light measurement messaging.
    /// </summary>
    public static class LightMeasurementInfrastructure
    {
        /// <summary>
        /// The name of the RabbitMQ exchange used for light measurements.
        /// </summary>
        public const string ExchangeName = "light.measured.exchange";
        /// <summary>
        /// The type of the RabbitMQ exchange used for light measurements.
        /// </summary>
        public const string ExchangeType = "direct";
        /// <summary>
        /// Whether the exchange is durable.
        /// </summary>
        public const bool ExchangeDurable = true;
        /// <summary>
        /// Whether the exchange is auto-delete.
        /// </summary>
        public const bool ExchangeAutoDelete = false;
        /// <summary>
        /// The name of the RabbitMQ queue used for light measurements.
        /// </summary>
        public const string QueueName = "light.measured.queue";
        /// <summary>
        /// Whether the queue is durable.
        /// </summary>
        public const bool QueueDurable = true;
        /// <summary>
        /// Whether the queue is auto-delete.
        /// </summary>
        public const bool QueueAutoDelete = false;
        /// <summary>
        /// The routing key used for light measurement messages.
        /// </summary>
        public const string RoutingKey = "light.measured";
    }
}
