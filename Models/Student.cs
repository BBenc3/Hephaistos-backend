using System;
using System.Collections.Generic;

namespace ProjectHephaistos.Models;

public partial class Student
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? MotherName { get; set; }

    public int? ClassId { get; set; }

    public string? EducationalId { get; set; }

    public sbyte? Gender { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? User { get; set; }
}
