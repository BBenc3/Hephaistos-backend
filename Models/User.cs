﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models
{
    public partial class User : IdentityUser<int>
    {
        public DateTime Created { get; set; } = DateTime.UtcNow; // Renamed property
        public string Role { get; set; } = "User";
        public bool Active { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
