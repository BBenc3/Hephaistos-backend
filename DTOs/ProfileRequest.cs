namespace ProjectHephaistos.DTOs
{
    /// <summary>
    /// Represents a request to update a user's profile.
    /// </summary>
    public class ProfileRequest
    {
        public string? Username { get; set; }

        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? Role { get; set; } = "User";

        public bool Active { get; set; }
    }
}
