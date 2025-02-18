using System.ComponentModel.DataAnnotations;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "A régi jelszó megadása kötelező.")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "Az új jelszó megadása kötelező.")]
    [MinLength(6, ErrorMessage = "Az új jelszónak legalább 6 karakter hosszúnak kell lennie.")]
    [MaxLength(100, ErrorMessage = "Az új jelszó legfeljebb 100 karakter lehet.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Az új jelszónak tartalmaznia kell legalább egy kisbetűt, egy nagybetűt és egy számot.")]
    public string NewPassword { get; set; }
}
