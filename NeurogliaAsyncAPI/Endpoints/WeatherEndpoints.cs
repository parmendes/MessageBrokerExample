using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RabbitMQAsyncAPI.Models;
using RabbitMQAsyncAPI.Services;

namespace RabbitMQAsyncAPI.Endpoints
{
    /// <summary>
    /// Weather API endpoints
    /// </summary>
    public static class WeatherEndpoints
    {
        /// <summary>
        /// Maps the weather endpoints to the application
        /// </summary>
        /// <param name="app"> The web application to map the endpoints to</param>
        /// <returns> The web application with the mapped endpoints</returns>
        public static void MapWeatherEndpoints(this WebApplication app)
        {
            app.MapPost("/api/v1/weather/signup",
                ([FromServices] RabbitMqProducerService producer, [FromBody] SignupRequest req) =>
                {
                    var evt = new UserSignedUpEvent(req.UserId, req.Email, DateTime.UtcNow);
                    var message = JsonSerializer.Serialize(evt);
                    _ = producer.PublishAsync(message);
                    return Results.Created($"/api/v1/weather/{req.UserId}", evt);
                })
                .WithName("UserSignup")
                .WithOpenApi();
        }
    }
}