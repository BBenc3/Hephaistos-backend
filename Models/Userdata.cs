using System.ComponentModel.DataAnnotations;

namespace ProjectHephaistos.Models;

public partial class Userdata
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string motherName { get; set; }
    public User User { get; set; }
}
