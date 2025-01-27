using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHephaistos.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Role { get; set; }

    public sbyte Active { get; set; }
}
