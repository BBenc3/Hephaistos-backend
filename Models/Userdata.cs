using System.ComponentModel.DataAnnotations;

namespace ProjectHephaistos.Models;

public partial class Userdata : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public User User { get; set; }
}
