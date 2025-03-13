using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public string? Role { get; set; }

    public string? Email { get; set; }

    public int? StartYear { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool? Active { get; set; }

    public string? Note { get; set; }

    public int? MajorId { get; set; }

    public string? ProfilePicturepath { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Auditlog> Auditlogs { get; set; } = new List<Auditlog>();

    public virtual ICollection<Completedsubject> Completedsubjects { get; set; } = new List<Completedsubject>();

    public virtual Major? Major { get; set; }

    public virtual ICollection<Refreshtoken> Refreshtokens { get; set; } = new List<Refreshtoken>();
}
