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
    public class SubjectsController : ControllerBase
    {
        private readonly HephaistosContext _context;

        public SubjectsController(HephaistosContext context)
        {
            _context = context;
        }

        // Bárki elérheti, csak az aktív tárgyak listázása
        [HttpGet]
        public IActionResult GetSubjects()
        {
            var subjects = _context.Subjects
                .Where(s => s.Active)
                .Include(s => s.Major)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Credit = x.CreditValue,
                    Semester = x.IsEvenSemester ? "Even" : "Odd",
                    Major = x.Major.Name
                })
                .ToList();

            return Ok(subjects);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddSubject([FromBody] AddSubjectRequest subject)
        {
            var newSubject = new Subject
            {
                Name = subject.Name,
                Code = subject.Code,
                CreditValue = subject.CreditValue,
                MajorId = subject.MajorId,
                IsElective = subject.IsElective,
                IsEvenSemester = subject.IsEvenSemester,
                Note = subject.Note,
                Active = true
            };

            try
            {
                _context.Subjects.Add(newSubject);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Subject added successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateSubject(int id, [FromBody] AddSubjectRequest subject)
        {
            var existing = _context.Subjects.FirstOrDefault(s => s.Id == id && s.Active);
            if (existing == null) return NotFound("Subject not found");

            existing.Name = subject.Name;
            existing.Code = subject.Code;
            existing.CreditValue = subject.CreditValue;
            existing.MajorId = subject.MajorId;
            existing.IsElective = subject.IsElective;
            existing.IsEvenSemester = subject.IsEvenSemester;
            existing.Note = subject.Note;

            _context.SaveChanges();
            return Ok("Subject updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSubject(int id)
        {
            var subject = _context.Subjects.FirstOrDefault(s => s.Id == id && s.Active);
            if (subject == null) return NotFound("Subject not found");

            subject.Active = false;
            _context.SaveChanges();

            return Ok("Subject logically deleted");
        }
    }
}
