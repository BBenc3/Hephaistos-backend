using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class ClassSchedule
{
    public int Id { get; set; }

    public int? SubjectId { get; set; }

    public int? Year { get; set; }

    public string? DayOfWeek { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Subject? Subject { get; set; }
}
