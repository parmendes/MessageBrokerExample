using Neuroglia.AsyncApi;
using RabbitMQAsyncAPI.Services;

namespace RabbitMQAsyncAPI.Configurations
{
    /// <summary>
    /// Configuration class for RabbitMQ options
    /// </summary>
    public static class RabbitMqConfiguration
    {
        /// <summary>
        /// Extension method to add RabbitMQ services to the service collection
        /// </summary>
        /// <param name="services"> The service collection to add the RabbitMQ services to</param>
        /// <param name="configuration"> The configuration object containing RabbitMQ settings</param>
        /// <returns> The updated service collection with RabbitMQ services</returns>
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMQ"));
            services.AddSingleton<RabbitMqProducerService>();
            return services;
        }
    }

    /// <summary>
    /// Configuration class for RabbitMQ options
    /// </summary>
    public class RabbitMqOptions
    {
        /// <summary>
        /// Gets or sets the RabbitMQ host name
        /// </summary>
        public string HostName { get; set; } = "localhost";
        /// <summary>
        /// Gets or sets the RabbitMQ user name
        /// </summary>
        public string UserName { get; set; } = "guest";
        /// <summary>
        /// Gets or sets the RabbitMQ password
        /// </summary>
        public string Password { get; set; } = "guest";
        /// <summary>
        /// Gets or sets the RabbitMQ exchange name
        /// </summary>
        public string Exchange { get; set; } = "weather_exchange";
        /// <summary>
        /// Gets or sets the RabbitMQ routing key
        /// </summary>
        public string RoutingKey { get; set; } = "weather_routing_key";
    }
}