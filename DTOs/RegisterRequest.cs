using ProjectHephaistos.Models;

namespace ProjectHephaistos.DTOs
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int StartYear { get; set; }
        public int MajorId { get; set; }
    }
}
