using ProjectHephaistos.Data;
using ProjectHephaistos.DTOs;
using ProjectHephaistos.Models;

namespace ProjectHephaistos.Services
{
    public class TimetableGenerator
    {
        private HephaistosContext context;
        public TimetableGenerator(HephaistosContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Generates a timetable for the given user.
        /// </summary>
        /// <param name="accessabbleLessons"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static TimetableResponse GenerateTimetable(List<Lesson> accessabbleLessons)
        {
            var response = new TimetableResponse();
            response.timetable = new List<Lesson>();
            response.junk = new List<Lesson>();

            throw new NotImplementedException();
        }
        /// <summary>
        /// Returns a list of lessons that the user has access to.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static List<Lesson> GetAccessibleLessons(User user)
        {

            throw new NotImplementedException();
        }
    }
}
