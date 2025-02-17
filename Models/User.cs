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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? Role { get; set; } = "User";

    public bool Active { get; set; }
}
