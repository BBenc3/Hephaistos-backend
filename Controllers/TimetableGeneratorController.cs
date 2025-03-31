using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHephaistos.Services;
using ProjectHephaistos.Data;
using System;
using Microsoft.AspNetCore.Authorization;
using ProjectHephaistos.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectHephaistos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableGeneratorController : ControllerBase
    {
        private readonly TimetableGenerator _timetableGenerator;
        private readonly JwtHelper _jwtHelper;
        private readonly HephaistosContext _context;

        // Dependency Injection a TimetableGenerator szolgáltatásra
        public TimetableGeneratorController(TimetableGenerator timetableGenerator, JwtHelper jwtHelper, HephaistosContext context)
        {
            _timetableGenerator = timetableGenerator;
            _jwtHelper = jwtHelper;
            _context = context;
        }

        // Védett végpont, ami meghívja a timetable generáló metódust
        [HttpPost("generate")]
        [Authorize] // Csak autentikált felhasználók férhetnek hozzá
        public async Task<IActionResult> GenerateTimetable([FromBody] int creditValue, [FromHeader(Name = "Authorization")] string Authorization)
        {
            try
            {
                var userId = _jwtHelper.ExtractUserIdFromToken(Authorization);
                if (userId == null)
                {
                    return Unauthorized("Invalid token.");
                }

                var user = _context.Users
                    .FirstOrDefault(u => u.Id == userId.Value);

                // Elérhető tantárgyak, ütemezés és előzőleg teljesített tantárgyak beszerzése
                var availableSubjects = GetAvailableSubjects(userId.Value); // Lekérés az adatbázisból
                var existingSchedules = _context.Subjectschedules
                    .AsEnumerable()
                    .Where(schedule =>
                        availableSubjects.Any(subject => subject.Id == schedule.SubjectId))
                    .ToList();

                // Timetable generálás
                var (timetable, omittedSubjects) = _timetableGenerator.GenerateClosestTimetable(creditValue, availableSubjects, existingSchedules);

                var simplifiedTimetable = timetable.Select(schedule => new
                {
                    SubjectName = schedule.Subject?.Name,
                    DayOfWeek = schedule.DayOfWeek,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime
                }).ToList();

                return Ok(new
                {
                    Timetable = simplifiedTimetable,
                    OmittedSubjects = omittedSubjects.Select(subject => new
                    {
                        SubjectName = subject.Name
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        // Az elérhető tantárgyak lekérdezése az adatbázisból
        private List<Subject> GetAvailableSubjects(int userId)
        {
            var user = _context.Users
                .Include(u => u.Major)
                .ThenInclude(m => m.Subjects)
                .Include(u => u.Completedsubjects)
                .FirstOrDefault(u => u.Id == userId);

            var availableSubjects = user.Major.Subjects
                .Where(subject =>
                    subject.PrerequisiteSubjects.All(prerequisite =>
                        user.Completedsubjects.Any(cs => cs.SubjectId == prerequisite.Id)))
                .ToList();

            return availableSubjects;
        }
    }
}
