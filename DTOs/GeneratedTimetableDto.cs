using System.Collections.Generic;
using ProjectHephaistos.Models;

namespace ProjectHephaistos.DTOs
{
    public class GeneratedTimetableDto
    {
        public IEnumerable<SubjectSchedule> Timetable { get; set; }
        public IEnumerable<Subject> OmittedSubjects { get; set; }
    }
}
