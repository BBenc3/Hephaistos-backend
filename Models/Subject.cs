﻿using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public int? CreditValue { get; set; }

    public int? MajorId { get; set; }

    public bool? IsElective { get; set; }

    public bool? IsEvenSemester { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? Active { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();

    public virtual Major? Major { get; set; }
}
