using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHephaistos.Models
{
    public partial class PickedLessons
    {
        [Key]
        public int Id { get; set; }

        // Külső kulcs a Student entitásra
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // Külső kulcs a Lesson entitásra
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}


