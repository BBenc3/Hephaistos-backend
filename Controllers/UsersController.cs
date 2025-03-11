using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProjectHephaistos.Data;
using ProjectHephaistos.DTOs;
using ProjectHephaistos.Models;
using ProjectHephaistos.Services;

namespace ProjectHephaistos.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly HephaistosContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly FtpService _ftpService;

        public UsersController(HephaistosContext context, JwtHelper jwtHelper, FtpService ftpService)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _ftpService = ftpService;
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser([FromHeader(Name = "Authorization")] string authorization)
        {
            var userId = _jwtHelper.ExtractUserIdFromToken(authorization);
            if (!userId.HasValue)
            {
                return Unauthorized("Invalid or expired token.");
            }
            var data = _context.Users.FirstOrDefault(c => c.Id == userId.Value);
            if (data == null)
            {
                return NotFound("User not found.");
            }
            ProfileRequest user = new ProfileRequest()
            {
                Username = data.UserName,
                Email = data.Email,
                Created = data.Created.Date,
                Role = data.Role,
                Active = data.Active
            };
            return Ok(user);
        }

        [HttpPut("me")]
        public IActionResult UpdateUser([FromHeader(Name = "Authorization")] string authorization, [FromForm] UserUpdateDto userDto)
        {
            var userId = _jwtHelper.ExtractUserIdFromToken(authorization);
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(c => c.Id == userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = userDto.Username;
            user.Email = userDto.Email;
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpDelete("me")]
        public IActionResult DeleteUser([FromHeader(Name = "Authorization")] string authorization)
        {
            var userId = _jwtHelper.ExtractUserIdFromToken(authorization);
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(c => c.Id == userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            user.Active = false;
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("me/profile-picture")]
        public IActionResult UploadProfilePicture([FromHeader(Name = "Authorization")] string authorization, IFormFile file)
        {
            var userId = _jwtHelper.ExtractUserIdFromToken(authorization);
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(c => c.Id == userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileName = $"{userId.Value}_{file.FileName}";
            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            _ftpService.UploadFile(filePath, fileName);
            user.ProfilePicturePath = fileName;
            _context.SaveChanges();

            return Ok(new { ProfilePicturePath = fileName });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("role/{id}")]
        public IActionResult UpdateUserRole(int id, [FromForm] string newRole)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Role = newRole;
            _context.SaveChanges();

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeactivateUser(int id)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Active = false;
            _context.SaveChanges();

            return Ok();
        }
    }
}
