using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectHephaistos.Models;

public partial class Course
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
