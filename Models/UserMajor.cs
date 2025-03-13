using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class UserMajor
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? MajorId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Major? Major { get; set; }

    public virtual User? User { get; set; }
}
