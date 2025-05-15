namespace RabbitMQAsyncAPI.Models
{
    /// <summary>
    /// /// Represents an event that is published when a user signs up  
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="Email"></param>
    /// <param name="Timestamp"></param>
    public record UserSignedUpEvent(string UserId, string Email, DateTime Timestamp);
}