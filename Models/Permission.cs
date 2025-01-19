using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public virtual ICollection<Rolepermission> Rolepermissions { get; set; } = new List<Rolepermission>();

    public virtual ICollection<Userpermission> Userpermissions { get; set; } = new List<Userpermission>();
}
