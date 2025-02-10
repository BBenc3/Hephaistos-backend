using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHephaistos.Models
{
    public partial class Lesson
    {
        [Key]
        public int Id { get; set; }

        // A tanóra neve
        public string ClassName { get; set; }

        // A tanóra kódja
        public string ClassCode { get; set; }

        // A tanóra leírása
        public string ClassDescription { get; set; }

        // A tanóra kezdési időpontja
        public DateTime StartDate { get; set; }

        // A tanóra befejezési időpontja
        public DateTime EndDate { get; set; }

        // A tanóra létrehozásának dátuma (alapértelmezett érték: UTC idő)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Külső kulcs a tanárhoz
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        // Sok-a-sok kapcsolat a diákokkal
        public List<PickedLessons> PickedLessons { get; set; }

        // Ha egy másik leckére épül, adjunk hozzá egy FK-t az előző tanórára
        public int? PreviousLessonId { get; set; }  // Az előző tanóra azonosítója (ha van)
        public Lesson PreviousLesson { get; set; }  // A kapcsolódó előző tanóra

        // Az új tanórákhoz tartozó kapcsolódások és adatbázis-kapcsolatok megtartása
    }
}
