using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Subject
{
    public int Id { get; set; }

    public int? TeacherId { get; set; }

    public int? ClassroomId { get; set; }

    public string? Name { get; set; }

    public DateOnly? Date { get; set; }

    public sbyte? IsCancelled { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Credits { get; set; }

    public virtual Classroom? Classroom { get; set; }

    public virtual Teacher? Teacher { get; set; }

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
