using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class University
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Place { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool? Active { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Major> Majors { get; set; } = new List<Major>();
}
