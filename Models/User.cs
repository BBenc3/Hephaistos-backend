using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public string? Role { get; set; }

    public string? Email { get; set; }

    public int? StartYear { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? Active { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<UserMajor> UserMajors { get; set; } = new List<UserMajor>();
}
