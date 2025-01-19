using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Rolepermission
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public int? PermissionId { get; set; }

    public virtual Permission? Permission { get; set; }

    public virtual Role? Role { get; set; }
}
