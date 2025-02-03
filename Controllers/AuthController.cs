using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ProjectHephaistos.Data;
using ProjectHephaistos.Models;

public class AuthController : ControllerBase
{
    private readonly HephaistosContext _context;
    private readonly JwtHelper _jwthelper;

    public AuthController(HephaistosContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwthelper = jwtHelper;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (_context.Users.Any(u => u.Email == request.Email))
            return BadRequest("Email already exists.");

        var (passwordHash, passwordSalt) = HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "Student",
            CreatedAt = DateTime.UtcNow,
            Active = true,
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);
        if (user == null)
            return Unauthorized("Invalid credentials.");

        var (computedHash, _) = HashPasswordWithSalt(request.Password, user.PasswordSalt);

        if (computedHash != user.PasswordHash)
            return Unauthorized("Invalid credentials.");

        // Generate an access token
        var accessToken = _jwthelper.GenerateToken(user.Id, user.Role);

        // Generate a refresh token
        var refreshToken = GenerateRefreshToken();
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            Expiration = DateTime.UtcNow.AddDays(7), // 7 days validity
            UserId = user.Id
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        _context.SaveChanges();

        // Return both access and refresh tokens
        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken // Include refresh token in response
        });
    }


    // #warning Unused method
    [HttpPost("refreshToken")]
    public IActionResult RefreshToken([FromBody] string refreshToken)
    {
        var tokenEntity = _context.RefreshTokens
            .SingleOrDefault(rt => rt.Token == refreshToken && rt.Expiration > DateTime.UtcNow);

        if (tokenEntity == null)
            return Unauthorized("Invalid or expired refresh token.");

        // Generálj új access tokent
        var accessToken = _jwthelper.GenerateToken(tokenEntity.UserId, tokenEntity.User.Role);

        // (Opcionálisan) Generálj új refresh tokent
        var newRefreshToken = GenerateRefreshToken();
        tokenEntity.Token = newRefreshToken;
        tokenEntity.Expiration = DateTime.UtcNow.AddDays(7);

        _context.SaveChanges();

        // Válasz vissza az új access és refresh tokennel
        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        });
    }

    // #warning Unused method
    [HttpPost("logout")]
    public IActionResult Logout([FromBody] LogoutRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            return BadRequest("Refresh token is required.");

        // Ellenőrizd, hogy a refresh token érvényes-e
        var storedToken = _context.RefreshTokens
            .SingleOrDefault(rt => rt.Token == request.RefreshToken);

        if (storedToken == null)
            return BadRequest("Invalid refresh token.");

        // Eltávolítjuk a refresh tokent az adatbázisból
        _context.RefreshTokens.Remove(storedToken);
        _context.SaveChanges();

        return Ok("User logged out successfully.");
    }


    private (string, string) HashPassword(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }
    }

    private (string, string) HashPasswordWithSalt(string password, string base64Salt)
    {
        var salt = Convert.FromBase64String(base64Salt);
        using (var hmac = new HMACSHA512(salt))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (Convert.ToBase64String(hash), base64Salt);
        }
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
