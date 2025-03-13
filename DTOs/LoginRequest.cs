namespace ProjectHephaistos.DTOs
{
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; }
        public string Email { get; set; } = null!;
        public bool StayLoggedIn { get; set; }
    }
}
