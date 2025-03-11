using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectHephaistos.Data;
using ProjectHephaistos.Models;
using ProjectHephaistos.DTOs; // Add this line to reference the DTOs namespace
using ProjectHephaistos.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHephaistos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly HephaistosContext _context;
        private readonly JwtHelper _jwthelper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly OtpService _otpService;
        private readonly EmailService _emailService;
        private readonly FtpService _ftpService;

        public AuthController(HephaistosContext context, JwtHelper jwtHelper, UserManager<User> userManager, IConfiguration configuration, OtpService otpService, EmailService emailService, FtpService ftpService)
        {
            _context = context;
            _jwthelper = jwtHelper;
            _userManager = userManager;
            _configuration = configuration;
            _otpService = otpService;
            _emailService = emailService;
            _ftpService = ftpService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                return BadRequest(new { message = "Email already exists." });

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                Role = "user",
                Active = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == "PasswordTooWeak"))
                {
                    return BadRequest(new { message = "Password is too weak" });
                }
                return BadRequest(result.Errors);
            }

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            if (!user.Active)
            {
                return Unauthorized("Inactive user. Contact admin.");
            }

            var jwtToken = _jwthelper.GenerateToken(user.Id, user.Role);

            if (loginRequest.StayLoggedIn)
            {
                var refreshToken = _jwthelper.GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
                SetRefreshTokenCookie(refreshToken.Token);
                return Ok(new
                {
                    Token = jwtToken,
                    RefreshToken = refreshToken.Token
                });
            }

            return Ok(new { Token = jwtToken });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromHeader(Name = "refreshToken")] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token is missing.");
            }

            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
            {
                return Unauthorized("Invalid token.");
            }

            var storedToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);
            if (storedToken == null || !storedToken.IsActive)
            {
                return Unauthorized("Invalid token.");
            }

            var newRefreshToken = _jwthelper.GenerateRefreshToken();
            storedToken.Revoked = DateTime.UtcNow;
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = _jwthelper.GenerateToken(user.Id, user.Role);
            SetRefreshTokenCookie(newRefreshToken.Token);

            return Ok(new
            {
                Token = jwtToken,
                RefreshToken = newRefreshToken.Token
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

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password changed successfully.");
        }

        [HttpPut("change-password-after-otp")]
        public async Task<IActionResult> ChangePasswordAfterOtp([FromBody] ChangePasswordAfterOtpRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!_otpService.VerifyOtp(request.Email, request.Otp))
            {
                return Unauthorized("Invalid OTP.");
            }

            try
            {
                var result = await _userManager.RemovePasswordAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                result = await _userManager.AddPasswordAsync(user, request.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("generate-otp")]
        public async Task<IActionResult> GenerateOtp([FromBody] OtpRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
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
                        <p>Kedves {user.UserName},</p>
                        <p>Az egyszeri hitelesítési kódod:</p>
                        <p style='font-size: 24px; font-weight: bold; color: {emailSettings.PrimaryColor};'>{otp}</p>
                        <p>Kérjük, használd ezt a kódot a hitelesítéshez.</p>
                        <p>Üdvözlettel,<br/>Team Hephaistos</p>
                        <p style='font-size: 12px; color: {emailSettings.TextColor};'>Kérem ne válaszoljon erre az emailre, ez egy automatikusan generált üzenet.</p>
                    </div>
                </body>
                </html>";

            await _emailService.SendEmailAsync(user.Email, user.UserName, subject, body, emailSettings);
            return Ok();
        }

        [HttpPost("validate-token")]
        public IActionResult ValidateToken([FromBody] TokenValidationRequest request)
        {
            var userId = _jwthelper.ExtractUserIdFromToken(request.Token);
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            return Ok(new { userId = user.Id, userName = user.UserName });
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
