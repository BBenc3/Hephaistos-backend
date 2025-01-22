using System;

namespace ProjectHephaistos.Models;

    public partial class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}


