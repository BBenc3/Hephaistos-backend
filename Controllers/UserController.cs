using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHephaistos.Data;
using ProjectHephaistos.DTOs;
using ProjectHephaistos.Models;
using ProjectHephaistos.Services;

namespace ProjectHephaistos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HephaistosContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;
        private readonly FtpService _ftpService;
        public UserController(HephaistosContext context, JwtHelper jwtHelper, IConfiguration configuration, FtpService ftpService)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
            _ftpService = ftpService;
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe([FromHeader(Name = "Authorization")] string Authorization)
        {
            var user = _context.Users
                .Include(u => u.Major)
                .ThenInclude(m => m.University)
                .FirstOrDefault(u => u.Id == _jwtHelper.ExtractUserIdFromToken(Authorization));
            if (user == null)
                return NotFound();

            var majorName = user.Major?.Name ?? "N/A";
            var universityName = user.Major?.University?.Name ?? "N/A";

            return Ok(new
            {
                username = user.Username,
                email = user.Email,
                startYear = user.StartYear,
                majorName = user.Major.Name,
                university = user.Major.University.Name,
                completedSubjects = user.Completedsubjects
            });
        }

        [HttpPut("completedSubjects")]
        [Authorize]
        public IActionResult completedSubjects([FromHeader(Name = "Authorization")] string authorization, [FromBody] AddCompletedSubjectRequest completedsubjects)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _jwtHelper.ExtractUserIdFromToken(authorization));
            if (user == null) return NotFound();

            foreach (var completedsubjectId in completedsubjects.completedSubjectIds)
            {
                var completedSubject = new Completedsubject
                {
                    UserId = user.Id,
                    SubjectId = completedsubjectId,
                };

                _context.Completedsubjects.Add(completedSubject);
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("completedSubjects")]
        [Authorize]
        public IActionResult GetCompletedSubjects([FromHeader(Name = "Authorization")] string authorization)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _jwtHelper.ExtractUserIdFromToken(authorization));
            if (user == null) return NotFound();
            var completedSubjects = _context.Completedsubjects.Where(cs => cs.UserId == user.Id).ToList();
            return Ok(completedSubjects);
        }

        [HttpPut("uploadProfilePicture")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture([FromHeader(Name = "Authorization")] string authorization, [FromForm] IFormFile file)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _jwtHelper.ExtractUserIdFromToken(authorization));
            if (user == null)
                return NotFound("Felhasználó nem található.");

            if (file == null || file.Length == 0)
                return BadRequest("Nem érkezett érvényes fájl.");

            var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var remoteFileName = $"{user.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            try
            {
                _ftpService.UploadFile(tempFilePath, remoteFileName);
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(tempFilePath);
                return StatusCode(500, $"Hiba történt a feltöltés során: {ex.Message}");
            }

            System.IO.File.Delete(tempFilePath);

            string ftpUrl = _configuration.GetValue<string>("FtpConfig:Url");
            var remoteFileUrl = $"{ftpUrl}/{remoteFileName}";

            user.ProfilePicturepath = remoteFileUrl;
            await _context.SaveChangesAsync();

            return Ok(new { fileUrl = remoteFileUrl });
        }



    }
}
