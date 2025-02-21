using System;

namespace ProjectHephaistos.Models
{
    public class RefreshToken : BaseModel
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= Expires;

        // Add reference to User model
        public int UserId { get; set; }
        public User User { get; set; }
    }
}


