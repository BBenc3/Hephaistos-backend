namespace ProjectHephaistos.DTOs
{
    public class ChangePasswordAfterOtpRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }
}
