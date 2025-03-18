using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHephaistos.Data;
using ProjectHephaistos.DTOs;
using ProjectHephaistos.Models;
using ProjectHephaistos.Services;

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
        [HttpGet]
        public async Task<IActionResult> GetMajors()
        {
            var majors = await _context.Majors
                .Include(m => m.University)
                .Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    University = new
                    {
                        Id = m.University.Id,
                        Name = m.University.Name
                    }
                })
                .ToListAsync();

            return Ok(majors);
        }

    }
}