namespace RabbitMQAsyncAPI.Models
{
    /// <summary>
    /// Represents an event that is published when a user signs up  
    /// </summary>
    public record UserSignedUpEvent(string UserId, string Email, DateTime Timestamp);
}