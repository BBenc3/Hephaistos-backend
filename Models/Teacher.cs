using System.ComponentModel.DataAnnotations;

namespace ProjectHephaistos.Models;

public partial class Teacher
{
    [Key]
    public int Id { get; set; }
    public string TeacherNumber { get; set; }
    public Lesson TeacherOfLesson { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
