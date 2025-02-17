using System.ComponentModel.DataAnnotations;

namespace ProjectHephaistos.Models;

public partial class Student
{
    [Key]
    public int Id { get; set; }
    public string StudentNumber { get; set; }
    public Course Course { get; set; }
    public User User { get; set; }
    public List<PickedLessons> StudentLessons { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
