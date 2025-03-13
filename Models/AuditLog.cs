using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Auditlog
{
    public int Id { get; set; }

    public string? TableName { get; set; }

    public int? RecordId { get; set; }

    public string? OperationType { get; set; }

    public string? ChangedData { get; set; }

    public DateTime ChangedAt { get; set; }

    public int? ChangedBy { get; set; }

    public virtual User? ChangedByNavigation { get; set; }
}
