using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHephaistos.Data;
using ProjectHephaistos.DTOs;
using ProjectHephaistos.Models;

namespace ProjectHephaistos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MajorsController : ControllerBase
    {
        private readonly HephaistosContext _context;

        public MajorsController(HephaistosContext context)
        {
            _context = context;
        }

        // Bárki elérheti, de csak az aktív szakokat listázza
        [HttpGet]
        public async Task<IActionResult> GetMajors()
        {
            var majors = await _context.Majors
                .Where(m => m.Active)
                .Include(m => m.University)
                .Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    Note = m.Note,
                    University = new
                    {
                        Id = m.University.Id,
                        Name = m.University.Name
                    }
                })
                .ToListAsync();

            return Ok(majors);
        }

        // Bárki elérheti, de csak az adott egyetem aktív szakait listázza
        [HttpGet("university/{universityId}")]
        public async Task<IActionResult> GetMajorsByUniversity(int universityId)
        {
            var majors = await _context.Majors
                .Where(m => m.Active && m.UniversityId == universityId)
                .Include(m => m.University)
                .Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    Note = m.Note,
                    University = new
                    {
                        Id = m.University.Id,
                        Name = m.University.Name
                    }
                })
                .ToListAsync();

            return Ok(majors);
        }

        // Csak admin adhat hozzá szakot
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMajor([FromBody] AddMajorRequest request)
        {
            var major = new Major
            {
                Name = request.Name,
                Note = request.Note,
                UniversityId = request.UniversityId,
                Active = true
            };

            _context.Majors.Add(major);
            await _context.SaveChangesAsync();

            return Ok(major);
        }

        // Csak admin módosíthat szakot
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMajor(int id, [FromBody] AddMajorRequest request)
        {
            var major = await _context.Majors.FirstOrDefaultAsync(m => m.Id == id && m.Active);
            if (major == null) return NotFound("Szak nem található.");

            major.Name = request.Name;
            major.Note = request.Note;
            major.UniversityId = request.UniversityId;

            await _context.SaveChangesAsync();

            return Ok("Szak módosítva.");
        }

        // Csak admin törölhet logikailag
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMajor(int id)
        {
            var major = await _context.Majors.FirstOrDefaultAsync(m => m.Id == id && m.Active);
            if (major == null) return NotFound("Szak nem található.");

            major.Active = false;
            await _context.SaveChangesAsync();

            return Ok("Szak logikailag törölve.");
        }
    }
}
