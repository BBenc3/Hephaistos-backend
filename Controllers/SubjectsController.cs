using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHephaistos.Data;
using ProjectHephaistos.DTOs;

namespace ProjectHephaistos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        HephaistosContext _context;

        public SubjectsController(HephaistosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSubjects()
        {
            return Ok(_context.Subjects.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Credit = x.CreditValue,
                Semester = x.IsEvenSemester ? "Even" : "Odd",
                Major = x.Major.Name
            }));
        }

        [HttpPost]
        public IActionResult AddSubject([FromBody] AddSubjectRequest subject)
        {
            var newSubject = new Models.Subject
            {
                Name = subject.Name,
                Code = subject.Code,
                CreditValue = subject.CreditValue,
                MajorId = subject.MajorId,
                IsElective = subject.IsElective,
                IsEvenSemester = subject.IsEvenSemester,
                Note = subject.Note
            };

            try{
                _context.Subjects.Add(newSubject);
                _context.SaveChanges();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
           
            return Ok("Subject added successfully");
        }
        //Módosítást majd csak admin jogosultsággal!
    }

}
