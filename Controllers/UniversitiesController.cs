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

        [HttpGet]
        public async Task<IActionResult> GetUniversities()
        {
            var universities = await _context.Universities
                .Include(u => u.Majors)
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
        public async Task<IActionResult> PostUniversity([FromForm] PostUniversityRequest request)
        {
            var newUniversity = new University
            {
                Name = request.Name,
                Note = request.Note,
                Place = request.Place
            };

            _context.Add(newUniversity);
            await _context.SaveChangesAsync();
            return Ok(newUniversity);

        }

        [HttpPost("AddMajors")]
        public async Task<IActionResult> AddMajors([FromBody] AddMajorsRequest request)
        {
            var university = await _context.Universities.FirstOrDefaultAsync(u => u.Id == request.UniversityId);
            if (university == null)
                return NotFound();
            foreach (var major in request.Majors)
            {
                var newMajor = new Major
                {
                    Name = major.Name,
                    Note = major.Note,
                    UniversityId = university.Id
                };
                _context.Majors.Add(newMajor);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
