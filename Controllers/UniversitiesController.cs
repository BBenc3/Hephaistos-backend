using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHephaistos.Data;
using ProjectHephaistos.DTOs;
using ProjectHephaistos.Models;

namespace ProjectHephaistos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversitiesController : ControllerBase
    {
        private readonly HephaistosContext _context;

        public UniversitiesController(HephaistosContext context)
        {
            _context = context;
        }

        // Bárki számára elérhető
        [HttpGet]
        public async Task<IActionResult> GetUniversities()
        {
            var universities = await _context.Universities
                .Where(u => u.Active)
                .Include(u => u.Majors.Where(m => m.Active))
                .Select(u => new
                {
                    Id = u.Id,
                    Name = u.Name,
                    Majors = u.Majors.Select(m => new
                    {
                        Id = m.Id,
                        Name = m.Name
                    }).ToList()
                })
                .ToListAsync();

            return Ok(universities);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostUniversity([FromForm] PostUniversityRequest request)
        {
            var newUniversity = new University
            {
                Name = request.Name,
                Note = request.Note,
                Place = request.Place,
                Active = true
            };

            _context.Add(newUniversity);
            await _context.SaveChangesAsync();
            return Ok(newUniversity);
        }

        [HttpPost("AddMajors")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMajors([FromBody] AddMajorsRequest request)
        {
            var university = await _context.Universities
                .FirstOrDefaultAsync(u => u.Id == request.UniversityId && u.Active);

            if (university == null)
                return NotFound();

            foreach (var major in request.Majors)
            {
                var newMajor = new Major
                {
                    Name = major.Name,
                    Note = major.Note,
                    UniversityId = university.Id,
                    Active = true
                };
                _context.Majors.Add(newMajor);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUniversity(int id, [FromForm] PostUniversityRequest request)
        {
            var university = await _context.Universities.FirstOrDefaultAsync(u => u.Id == id && u.Active);
            if (university == null) return NotFound();

            university.Name = request.Name;
            university.Note = request.Note;
            university.Place = request.Place;

            await _context.SaveChangesAsync();
            return Ok("University updated");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUniversity(int id)
        {
            var university = await _context.Universities.FirstOrDefaultAsync(u => u.Id == id && u.Active);
            if (university == null) return NotFound();

            university.Active = false;

            await _context.SaveChangesAsync();
            return Ok("University logically deleted");
        }
    }
}
