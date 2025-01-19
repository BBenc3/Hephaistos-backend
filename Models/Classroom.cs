using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Classroom
{
    public int Id { get; set; }

    public int? Capacity { get; set; }

    public int? ResponsibleTeacherId { get; set; }

    public double? Size { get; set; }

    public sbyte? Projector { get; set; }

    public sbyte? Smartboard { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Teacher? ResponsibleTeacher { get; set; }

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
