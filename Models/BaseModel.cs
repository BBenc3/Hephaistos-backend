using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectHephaistos.Models
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; } // Primary key
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
