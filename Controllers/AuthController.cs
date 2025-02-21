using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectHephaistos.Data;
using ProjectHephaistos.Models;
using ProjectHephaistos.DTOs; // Add this line to reference the DTOs namespace
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

        public AuthController(HephaistosContext context, JwtHelper jwtHelper, UserManager<User> userManager, IConfiguration configuration)
        {
            _context = context;
            _jwthelper = jwtHelper;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                return BadRequest("Email already exists.");

            var user = new User
            {
                UserName = request.Email, // Use UserName property from IdentityUser
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
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            var jwtToken = _jwthelper.GenerateToken(user.Id, user.Role);
            var refreshToken = _jwthelper.GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            SetRefreshTokenCookie(refreshToken.Token);

            return Ok(new
            {
                Token = jwtToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token is missing.");
            }

            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
            {
                return Unauthorized("Invalid token.");
            }

            var storedToken = user.RefreshTokens.Single(x => x.Token == refreshToken);
            if (!storedToken.IsActive)
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
                Token = jwtToken
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

            // Verify OTP (this is a placeholder, implement your OTP verification logic here)
            if (!VerifyOtp(request.Email, request.Otp))
            {
                return Unauthorized("Invalid OTP.");
            }

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

        private bool VerifyOtp(string email, string otp)
        {
            // Implement your OTP verification logic here
            return true;
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
