using Microsoft.AspNetCore.Mvc;
using ProjectHephaistos.Data;

namespace ProjectHephaistos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HephaistosContext _context;
        private readonly JwtHelper _jwtHelper;
        public UserController(HephaistosContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }
        [HttpGet("me")]
        public IActionResult GetMe([FromHeader(Name = "Authorization")] string Authorization)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _jwtHelper.ExtractUserIdFromToken(Authorization));
            var university = _context.Universities.FirstOrDefault(u => u.Id == user.UserMajors.FirstOrDefault().Major.UniversityId);
            var major = _context.Majors.FirstOrDefault(m => m.Id == user.UserMajors.FirstOrDefault().MajorId);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                username = user.Username,
                email = user.Email,
                startYear = user.StartYear,
                nameOfUniversity = university.Name,
                major = major.Name
                
            });
        }

    }
}
