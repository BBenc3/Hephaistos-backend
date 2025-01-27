using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectHephaistos.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using ProjectHephaistos.Data;

namespace ProjectHephaistos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HephaistosContext _context;
        private readonly JwtHelper _jwtHelper;

        public UsersController(HephaistosContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        // This endpoint will return the logged-in user's data
        [HttpGet("me")]
        [Authorize] // Ensure the user is authenticated
        public IActionResult GetCurrentUser([FromHeader(Name = "Authorization")] string authorization)
        {
        
            var userId = _jwtHelper.ExtractUserIdFromToken(authorization);

            if (!userId.HasValue)
            {
                return Unauthorized("Invalid or expired token.");
            }

            // Step 3: Retrieve user from database
            var user = _context.Users.FirstOrDefault(c => c.Id == userId.Value);

            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [HttpPut("me")]
        public IActionResult UpdateUser([FromHeader(Name = "Authorization")] string authorization, [FromForm] UserUpdateDto userDto)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == _jwtHelper.ExtractUserIdFromToken(authorization).Value);

            if (user == null)
            {
                return NotFound();
            }

            user.Username = userDto.Username;
            user.Email = userDto.Email;

            _context.SaveChanges();

            return Ok(user);
        }
    

    [Authorize]
    [HttpDelete]
    public IActionResult DeleteUser([FromHeader(Name = "Authorization")] string authorization)
    {
        var user = _context.Users.FirstOrDefault(c => c.Id == _jwtHelper.ExtractUserIdFromToken(authorization).Value);

        if (user == null)
        {
            return NotFound();
        }

        _context.Users.FirstOrDefault(c => c.Id == user.Id).Active = 0;
        _context.SaveChanges();

        return Ok();
    }
   

   
    }

}
