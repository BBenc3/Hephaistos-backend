﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Icao;
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
                .Include(u => u.Completedsubjects)
                .ThenInclude(cs => cs.Subject)
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
                profilePicturePath = user.ProfilePicturepath,
                completedSubjects = user.Completedsubjects.Select(x => new { x.SubjectId, x.Subject.Name }).ToList(),
            });
        }

        [HttpPut("completedSubjects")]
        [Authorize]
        public IActionResult completedSubjects([FromHeader(Name = "Authorization")] string authorization, [FromBody] AddCompletedSubjectRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _jwtHelper.ExtractUserIdFromToken(authorization));
            if (user == null) return NotFound();

            user.Completedsubjects = request.completedSubjectIds.Select(cs => new Completedsubject
            {
                UserId = user.Id,
                SubjectId = cs
            }).ToList();

            _context.SaveChanges();

            return Ok();
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

        [HttpPut("changeProfilePicture")]
        [Authorize]
        public async Task<IActionResult> ChangeProfilePicture([FromHeader(Name = "Authorization")] string authorization, [FromBody] ChangeProfilePictureRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _jwtHelper.ExtractUserIdFromToken(authorization));
            if (user == null) return NotFound("User not found.");

            string ftpUrl = _configuration.GetValue<string>("FtpConfig:Url");
            user.ProfilePicturepath = $"{ftpUrl}/{request.ProfilePicturePath}";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile picture updated successfully.", fileUrl = user.ProfilePicturepath });
        }

        public class ChangeProfilePictureRequest
        {
            public string ProfilePicturePath { get; set; }
        }

        [HttpGet("subjects")]
        [Authorize]
        public IActionResult GetSubjects([FromHeader(Name = "Authorization")] string authorization)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _jwtHelper.ExtractUserIdFromToken(authorization));
            if (user == null) return NotFound();

            var subjects = _context.Subjects
                .Include(s => s.Major)
                .ThenInclude(m => m.University)
                .Where(s => s.MajorId == user.MajorId)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    MajorName = s.Major.Name,
                    UniversityName = s.Major.University.Name
                })
                .ToList();

            return Ok(subjects);
        }

        // Admin only endpoints
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users
                .Include(u => u.Major)
                .Select(u => new UserAdminDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    StartYear = u.StartYear,
                    Role = u.Role,
                    Note = u.Note,
                    Active = u.Active,
                    Status = u.Status,
                    MajorId = u.MajorId
                }).ToList();

            return Ok(users);
        }

        [HttpGet("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.Include(u => u.Major).FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            return Ok(new UserAdminDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                StartYear = user.StartYear,
                Role = user.Role,
                Note = user.Note,
                Active = user.Active,
                Status = user.Status,
                MajorId = user.MajorId
            });
        }

        [HttpPost("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser([FromBody] CreateUserDTO dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
                return Conflict("Már létezik ilyen felhasználónév.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                StartYear = dto.StartYear ?? DateTime.Now.Year,
                Role = dto.Role ?? "User",
                CreatedAt = DateTime.UtcNow,
                Active = true,
                MajorId = dto.MajorId
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Felhasználó sikeresen létrehozva.", userId = user.Id });
        }

        [HttpPut("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            user.Email = dto.Email ?? user.Email;
            user.Role = dto.Role ?? user.Role;
            user.Note = dto.Note ?? user.Note;
            user.Status = dto.Status ?? user.Status;
            user.StartYear = dto.StartYear ?? user.StartYear;
            user.Active = dto.Active ?? user.Active;
            user.MajorId = dto.MajorId ?? user.MajorId;

            _context.SaveChanges();

            return Ok(new { message = "Felhasználó frissítve." });
        }

        [HttpDelete("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            user.Active = false;
            _context.SaveChanges();

            return Ok(new { message = "Felhasználó törölve." });
        }

    }
}
