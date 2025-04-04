using System;


namespace ProjectHephaistos.DTOs
{
    public class UserAdminDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public int? StartYear { get; set; }
        public string? Role { get; set; }
        public string? Note { get; set; }
        public bool? Active { get; set; }
        public string? Status { get; set; }
        public int? MajorId { get; set; }
    }

    public class CreateUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? StartYear { get; set; }
        public string? Role { get; set; }
        public int? MajorId { get; set; }
    }

    public class UpdateUserDTO
    {
        public string? Email { get; set; }
        public string? Role { get; set; }
        public int? StartYear { get; set; }
        public string? Note { get; set; }
        public bool? Active { get; set; }
        public string? Status { get; set; }
        public int? MajorId { get; set; }
    }
}
