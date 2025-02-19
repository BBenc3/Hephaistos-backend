using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectHephaistos.Data;
using ProjectHephaistos.Models;
using System.Security.Cryptography;
using System.Text;

[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly HephaistosContext _context;
    private readonly JwtHelper _jwthelper;
    private readonly UserManager<User> _userManager;

    public AuthController(HephaistosContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwthelper = jwtHelper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (_context.Users.Any(u => u.Email == request.Email))
            return BadRequest("Email already exists.");

        var user = new User
        {
            
            Email = request.Email,
            Role = "user",
            Active = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);
        if (user == null)
            return Unauthorized("Invalid credentials.");
        if (user.Active == false)
        {
            return Unauthorized("Inactive user.");
        }

        var (computedHash, _) = HashPasswordWithSalt(request.Password, user.PasswordSalt);

        if (computedHash != user.PasswordHash)
            return Unauthorized("Invalid credentials.");

        var accessToken = _jwthelper.GenerateToken(user.Id, user.Role);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            Expiration = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        _context.SaveChanges();

        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost("refreshToken")]
    public IActionResult RefreshToken([FromBody] string refreshToken)
    {
        var tokenEntity = _context.RefreshTokens
            .SingleOrDefault(rt => rt.Token == refreshToken && rt.Expiration > DateTime.UtcNow);

        if (tokenEntity == null)
            return Unauthorized("Invalid or expired refresh token.");

        var accessToken = _jwthelper.GenerateToken(tokenEntity.UserId, tokenEntity.User.Role);
        var newRefreshToken = GenerateRefreshToken();
        tokenEntity.Token = newRefreshToken;
        tokenEntity.Expiration = DateTime.UtcNow.AddDays(7);

        _context.SaveChanges();

        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout([FromBody] LogoutRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            return BadRequest("Refresh token is required.");

        var storedToken = _context.RefreshTokens.SingleOrDefault(rt => rt.Token == request.RefreshToken);

        if (storedToken == null)
            return BadRequest("Invalid refresh token.");

        _context.RefreshTokens.Remove(storedToken);
        _context.SaveChanges();

        return Ok("User logged out successfully.");
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

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("Felhasználó nem található.");
        }

        var passwordCheck = await _userManager.CheckPasswordAsync(user, request.OldPassword);
        if (!passwordCheck)
        {
            return BadRequest("A megadott régi jelszó helytelen.");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Jelszó sikeresen módosítva.");
    }
}
