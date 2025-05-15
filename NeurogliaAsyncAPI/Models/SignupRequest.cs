namespace RabbitMQAsyncAPI.Models
{
    /// <summary>
    /// Represents a request to sign up a user
    /// </summary>
    public class SignupRequest
    {
        /// <summary>
        /// Gets or sets the user ID. This field is required.
        /// </summary>
        public required string UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the email address of the user. This field is required.
        /// </summary>
        public required string Email { get; set; }
    }
}