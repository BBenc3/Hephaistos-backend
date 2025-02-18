using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectHephaistos.Data;
using ProjectHephaistos.DTOs;

namespace ProjectHephaistos.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly HephaistosContext _context;
        private readonly JwtHelper _jwtHelper;

        public UsersController(HephaistosContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
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
            ProfileRequest user = new ProfileRequest()
            {
                Username = data.Username,
                Email = data.Email,
                CreatedAt = data.CreatedAt.Date,
                Role = data.Role,
                Active = data.Active

            };
            if (user == null)
            {
                return NotFound("User not found.");
            }
           
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

            user.Username = userDto.Username;
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
