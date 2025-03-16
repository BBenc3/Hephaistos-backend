namespace ProjectHephaistos.DTOs
{
    public class LoginRequest
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
        public bool StayLoggedIn { get; set; }
    }
}
