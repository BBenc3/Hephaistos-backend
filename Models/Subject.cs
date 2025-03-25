using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models
{
    public partial class Subject
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public int? CreditValue { get; set; }

        public int? MajorId { get; set; }

        public bool IsElective { get; set; } = false;

        public bool IsEvenSemester { get; set; } = DateTime.Now.Month <= 8;

        public DateTime CreatedAt { get; set; }

        public bool? Active { get; set; }

        public string? Note { get; set; }

        public virtual ICollection<SubjectSchedule> Subjectschedules { get; set; } = new List<SubjectSchedule>();

        public virtual ICollection<Completedsubject> Completedsubjects { get; set; } = new List<Completedsubject>();

        public virtual Major? Major { get; set; }

        // Új kapcsolat: Előfeltételek (Many-to-Many)
        public virtual ICollection<Subject> PrerequisiteSubjects { get; set; } = new List<Subject>();

        public virtual ICollection<Subject> RequiredForSubjects { get; set; } = new List<Subject>();
    }
}
