using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? MotherName { get; set; }

    public DateOnly? EmploymentStartDate { get; set; }

    public sbyte? Gender { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();

    public virtual User? User { get; set; }
}
