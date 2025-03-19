namespace ProjectHephaistos.DTOs
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int StartYear { get; set; } = 0000;
        public int MajorId { get; set; } = 1;
        public string Status { get; set; } = "Active";
    }
}
