namespace ProjectHephaistos.DTOs
{
    public class AddMajorRequest
    {
        public string Name { get; set; } = null!;
        public string? Note { get; set; }
        public int UniversityId { get; set; }
    }
}
