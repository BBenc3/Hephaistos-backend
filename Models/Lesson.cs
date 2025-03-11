using System.ComponentModel.DataAnnotations;

namespace ProjectHephaistos.Models
{
    public partial class LessonTopic : BaseModel
    {
        [Required]
        public string TopicName { get; set; }
        [Required]
        public string TopicCode { get; set; }
        [Required]
        public string TopicDescription { get; set; }

    }
}
