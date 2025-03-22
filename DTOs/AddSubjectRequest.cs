using System;

namespace ProjectHephaistos.DTOs;

public class AddSubjectRequest
{
    public string Name { get; set; }
    public string Code { get; set; }
    public int CreditValue { get; set; }
    public int MajorId { get; set; }
    public bool IsElective { get; set; }
    public bool IsEvenSemester { get; set; }
    public string Note { get; set; }
}
