using Neuroglia.AsyncApi;

namespace RabbitMQAsyncAPI.Configurations
{
    /// <summary>
    /// Configuration class for setting up AsyncAPI documentation.
    /// </summary>
    public static class AsyncApiConfiguration
    {
        /// <summary>
        /// Adds and configures the AsyncAPI document builder.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddAsyncApiConfiguration(this IServiceCollection services)
        {
            // Add the AsyncAPI document builder to the service collection
            // and configure it to use the AsyncAPI V3 specification.
            services.AddAsyncApi();               

            // Register AsyncAPI services
            services.AddAsyncApiGeneration(builder => 
                builder.UseDefaultV3DocumentConfiguration(asyncApi  =>
                {
                    asyncApi.WithSpecVersion("3.0.0");

                    asyncApi.WithTitle("Weather API")
                        .WithVersion("1.0.0")
                        .WithDescription("Weather API with RabbitMQ and AsyncAPI");

                    // Configure RabbitMQ server
                    asyncApi.WithServer("rabbitmq", server => server
                        .WithHost("localhost")
                        .WithPathName("/")
                        .WithProtocol("amqp")
                        .WithDescription("RabbitMQ server for the Weather API"));

                    // Configure channels
                    asyncApi.WithChannel("user/signedup", channel => channel
                        .WithAddress("user/signedup")
                        .WithDescription("Emits an event when a user signs up")
                        .WithMessage("UserSignedUpMessage", message => message
                            .WithTitle("User signed up")
                            .WithDescription("Emits an event when a user signs up")
                            .WithContentType("application/json")));
                })
            );

            return services;
        }
    }
}