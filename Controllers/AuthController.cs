using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectHephaistos.Data;
using ProjectHephaistos.Models;
using ProjectHephaistos.DTOs;
using ProjectHephaistos.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectHephaistos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly HephaistosContext _context;
        private readonly JwtHelper _jwthelper;
        private readonly IConfiguration _configuration;
        private readonly OtpService _otpService;
        private readonly EmailService _emailService;
        private readonly FtpService _ftpService;

        public AuthController(HephaistosContext context, JwtHelper jwtHelper, IConfiguration configuration, OtpService otpService, EmailService emailService, FtpService ftpService)
        {
            _context = context;
            _jwthelper = jwtHelper;
            _configuration = configuration;
            _otpService = otpService;
            _emailService = emailService;
            _ftpService = ftpService;
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                return BadRequest("Ezzel az email címmel már regisztráltak.");

            var usersMajor = await _context.Majors.SingleOrDefaultAsync(m => m.Id == request.MajorId);
            if (usersMajor == null)
                return BadRequest("Nem található a megadott szak.");

            string salt = GenerateSalt();
            string hashedPassword = HashPassword(request.Password, salt);

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                Role = "User",
            };

            _context.UserMajors.Add(new UserMajor
            {
                User = newUser,
                Major = usersMajor
            });

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok("Sikeres regisztráció.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return BadRequest("Hibás felhasználónév vagy jelszó.");

            string hashedInputPassword = HashPassword(request.Password, user.PasswordSalt);
            if (hashedInputPassword != user.PasswordHash)
                return BadRequest("Hibás felhasználónév vagy jelszó.");

            if (user.Active == false)
                return BadRequest("A felhasználó nem található");

            if (request.StayLoggedIn == false)
                return Ok(new { accessToken = _jwthelper.GenerateToken(user.Id, user.Role) });

            return Ok(new
            {
                accessToken = _jwthelper.GenerateToken(user.Id, user.Role),
                refreshToken = _jwthelper.GenerateRefreshToken()
            });
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, [FromHeader(Name = "Authorization")] string Authorization)
        {
            _jwthelper.ValidateToken(Authorization);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == _jwthelper.ExtractUserIdFromToken(Authorization));

            string hashedOldPassword = HashPassword(request.OldPassword, user.PasswordSalt);
            if (hashedOldPassword != user.PasswordHash)
                return BadRequest("Hibás jelszó.");

            string newSalt = GenerateSalt();
            user.PasswordSalt = newSalt;
            user.PasswordHash = HashPassword(request.NewPassword, newSalt);

            await _context.SaveChangesAsync();
            return Ok("Jelszó sikeresen megváltoztatva.");
        }


        [HttpPost("generate-otp")]
        public async Task<IActionResult> GenerateOtp([FromBody] OtpRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Nincs regisztrált felhasználó ezzel az email címmel.");
            }

            var otp = await _otpService.GenerateOtpAsync(request.Email);
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();
            var subject = "Egyszeri hitelesítési kód";
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: {emailSettings.BackgroundColor}; color: {emailSettings.TextColor};'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid {emailSettings.BorderColor}; border-radius: 10px;'>
                        <h2 style='color: {emailSettings.PrimaryColor};'>Egyszeri hitelesítési kód</h2>
                        <p>Kedves {user.Username},</p>
                        <p>Az egyszeri hitelesítési kódod:</p>
                        <p style='font-size: 24px; font-weight: bold; color: {emailSettings.PrimaryColor};'>{otp}</p>
                        <p>Kérjük, használd ezt a kódot a hitelesítéshez.</p>
                        <p>Üdvözlettel,<br/>Team Hephaistos</p>
                        <p style='font-size: 12px; color: {emailSettings.TextColor};'>Kérem ne válaszoljon erre az emailre, ez egy automatikusan generált üzenet.</p>
                    </div>
                </body>
                </html>";

            await _emailService.SendEmailAsync(user.Email, user.Username, subject, body, emailSettings);
            return Ok();
        }

        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Nincs regisztrált felhasználó ezzel az email címmel.");
            }
          
            if (_otpService.VerifyOtp(request.Email, request.Otp))
            {
                return BadRequest("Hibás hitelesítési kód.");
            }

            string newSalt = GenerateSalt();
            user.PasswordSalt = newSalt;
            user.PasswordHash = HashPassword(request.NewPassword, newSalt);
            await _context.SaveChangesAsync();
            return Ok("Jelszó sikeresen megváltoztatva.");
        }
    }
}
    



