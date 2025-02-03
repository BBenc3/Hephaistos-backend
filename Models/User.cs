using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHephaistos.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Role { get; set; }

    public bool Active { get; set; }
}
