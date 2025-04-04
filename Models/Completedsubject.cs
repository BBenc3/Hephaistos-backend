using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Completedsubject
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? SubjectId { get; set; }

    public bool Active { get; set; } = true;

    public virtual Subject? Subject { get; set; }

    public virtual User? User { get; set; }
}
