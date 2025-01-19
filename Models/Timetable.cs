using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Timetable
{
    public int Id { get; set; }

    public int? TeacherId { get; set; }

    public int? SubjectId { get; set; }

    public int? ClassroomId { get; set; }

    public string? Semester { get; set; }

    public int? CreditsEarned { get; set; }

    public virtual Classroom? Classroom { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
