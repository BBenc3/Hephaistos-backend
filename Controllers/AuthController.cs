using System;
using System.Linq;
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
            Role = "Student", // or set based on your requirements
            CreatedAt = DateTime.UtcNow
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

        var token = _jwthelper.GenerateToken(user.Id, user.Role);

        return Ok(new { Token = token });
    }

    [HttpPost("validateToken")]
    public IActionResult ValidateToken([FromBody] string token)
    {
        if (string.IsNullOrEmpty(token))
            return BadRequest("Token is required.");

        try
        {
            var principal = _jwthelper.ValidateToken(token);
            return Ok("valid");
        }
        catch (Exception ex)
        {
            return BadRequest($"Token validation failed: {ex.Message}");
        }
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

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
