using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Major
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? UniversityId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? Active { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    public virtual University? University { get; set; }

    public virtual ICollection<UserMajor> UserMajors { get; set; } = new List<UserMajor>();
}
