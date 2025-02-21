using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models
{
    public class User : IdentityUser<int>
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "User";
        public bool Active { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
