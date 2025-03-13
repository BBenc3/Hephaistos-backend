using ProjectHephaistos.Models;

namespace ProjectHephaistos.DTOs
{
    public class AddMajorsRequest
    {
        public List<MajorDTO> Majors { get; set; }
        public int UniversityId { get; set; }
    }

    public class MajorDTO
    {
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
