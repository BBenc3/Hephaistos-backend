using ProjectHephaistos.Models;

namespace ProjectHephaistos.DTOs
{
    public class TimetableResponse
    {
        public List<Lesson> timetable { get; set; }
        public List<Lesson> junk { get; set; }
    }
}
