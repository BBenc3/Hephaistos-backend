using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models
{
    public partial class GeneratedTimetable
    {
        public int Id { get; set; }

        public int UserId { get; set; } // Melyik felhasználóhoz tartozik az órarend

        public string Name { get; set; } = string.Empty; // Opcionális elnevezés az órarendnek

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Mikor készült az órarend

        public bool? Active { get; set; } = true; // Aktív-e az órarend

        public virtual User? User { get; set; } // Kapcsolat a felhasználóhoz

        public virtual ICollection<SubjectSchedule> ClassSchedules { get; set; } = new List<SubjectSchedule>(); // Kapcsolat az órarendi bejegyzésekkel
    }
}
