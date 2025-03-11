using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectHephaistos.Data;
using ProjectHephaistos.Models;
using ProjectHephaistos.DTOs;
using ProjectHephaistos.Services;

namespace ProjectHephaistos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly HephaistosContext _context;

        public LessonsController(HephaistosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetLessons()
        {
            var lessons = _context.Lessons.ToList();
            var lessonResponses = new List<LessonTopicResponse>();
            foreach (var lesson in lessons)
            {
                lessonResponses.Add(new LessonTopicResponse
                {
                    TopicName = lesson.TopicName,
                    TopicCode = lesson.TopicCode,
                    TopicDescription = lesson.TopicDescription
                });
            }

            return Ok(lessonResponses);
        }

        [HttpPost]
        public IActionResult CreateLesson([FromBody] LessonTopicRequest lessonRequest)
        {
            var lesson = new LessonTopic
            {
                TopicName = lessonRequest.TopicName,
                TopicCode = lessonRequest.TopicCode,
                TopicDescription = lessonRequest.TopicDescription
            };

            _context.Lessons.Add(lesson);
            _context.SaveChanges();

            return Ok();
        }
    }
}