using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHephaistos.Data;
using ProjectHephaistos.DTOs;
using ProjectHephaistos.Models;
using ProjectHephaistos.Services;

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (emailExists)
                return BadRequest("Ezzel az email címmel már regisztráltak.");

            var userMajor = await _context.Majors.SingleOrDefaultAsync(m => m.Id == request.MajorId);
            if (userMajor == null)
                return BadRequest("Nem található a megadott szak.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Role = "User",
                MajorId = request.MajorId,
                Status = request.Status,
                StartYear = request.StartYear,
                ProfilePicturepath = "default.png",
                Major = userMajor
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok("Sikeres regisztráció.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.UsernameOrEmail);
            if (user == null)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail);

            if (user == null)
                return BadRequest("Hibás felhasználónév vagy jelszó.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
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

            if (user == null)
                return NotFound("Felhasználó nem található.");

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
                return BadRequest("Hibás jelszó.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();
            return Ok("Jelszó sikeresen megváltoztatva.");
        }

        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Nincs regisztrált felhasználó ezzel az email címmel.");
            }

            if (!_otpService.VerifyOtp(request.Email, request.Otp))
            {
                return BadRequest("Hibás hitelesítési kód.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
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

            // Provide required parameters for SendEmailAsync
            var subject = "Egyszeri hitelesítési kód";
            var body = $"Az egyszeri hitelesítési kódod: {otp}";
            var senderEmail = _configuration.GetValue<string>("EmailSettings:FromAddress");
            var senderName = _configuration.GetValue<string>("EmailSettings:FromName");

            await _emailService.SendEmailAsync(request.Email, subject, body, senderEmail, senderName);

            return Ok("Az egyszeri hitelesítési kód elküldve az email címre.");
        }
    }
}
