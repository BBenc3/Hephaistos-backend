using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Userpermission
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? PermissionId { get; set; }

    public virtual Permission? Permission { get; set; }

    public virtual User? User { get; set; }
}
