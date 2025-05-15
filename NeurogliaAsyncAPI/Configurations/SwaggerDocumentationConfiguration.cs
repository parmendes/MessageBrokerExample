using Microsoft.OpenApi.Models;

namespace RabbitMQAsyncAPI.Configurations
{
    /// <summary>
    /// Configuration class for Swagger documentation
    /// </summary>
    public static class SwaggerDocumentationConfiguration
    {
        /// <summary>
        /// Extension method to add Swagger documentation to the service collection
        /// </summary>
        /// <param name="services"> The service collection to add Swagger documentation to</param>
        /// <returns> The updated service collection with Swagger documentation</returns>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather API", Version = "v1" });
            });
            return services;
        }
    }
}