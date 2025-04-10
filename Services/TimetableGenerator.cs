using System;
using System.Collections.Generic;
using System.Linq;
using ProjectHephaistos.Models;

namespace ProjectHephaistos.Services
{
    public class TimetableGenerator
    {
        public TimetableGenerator() { }

        public (IEnumerable<SubjectSchedule> Timetable, IEnumerable<Subject> OmittedSubjects) GenerateClosestTimetable(int creditValue, List<Subject> _availableSubjects, List<SubjectSchedule> _existingSchedules)
        {
            var selectedSubjects = new List<Subject>();
            var omittedSubjects = new List<Subject>();
            var selectedSchedules = new List<SubjectSchedule>();
            int totalCredits = 0;

            var subjectDependencyCount = _availableSubjects
                .Where(s => s.RequiredForSubjects.Any())
                .ToDictionary(s => s.Id, s => s.RequiredForSubjects.Count);

            var sortedSubjects = _availableSubjects
                .OrderByDescending(s => subjectDependencyCount.ContainsKey(s.Id) ? subjectDependencyCount[s.Id] : 0)
                .ThenByDescending(s => s.CreditValue ?? 0)
                .ToList();

            foreach (var subject in sortedSubjects)
            {
                if (totalCredits + (subject.CreditValue ?? 0) > creditValue)
                {
                    omittedSubjects.Add(subject);
                    continue;
                }

                var subjectSchedules = _existingSchedules.Where(s => s.SubjectId == subject.Id).ToList();

                if (!subjectSchedules.Any(schedule => IsOverlapping(schedule, selectedSchedules)))
                {
                    selectedSubjects.Add(subject);
                    selectedSchedules.AddRange(subjectSchedules);
                    totalCredits += subject.CreditValue ?? 0;
                }
                else
                {
                    omittedSubjects.Add(subject);
                }
            }

            // Ensure unique schedules based on subject name, day, start and end time.
            var uniqueSchedules = selectedSchedules
                .GroupBy(s => new { s.Subject?.Name, s.DayOfWeek, s.StartTime, s.EndTime })
                .Select(g => g.First())
                .ToList();

            // Ensure unique omitted subjects based on subject name.
            var uniqueOmittedSubjects = omittedSubjects
                .GroupBy(s => s.Name)
                .Select(g => g.First())
                .ToList();

            return (uniqueSchedules, uniqueOmittedSubjects);
        }

        private bool IsOverlapping(SubjectSchedule newSchedule, List<SubjectSchedule> existingSchedules)
        {
            return existingSchedules.Any(existing =>
                existing.DayOfWeek == newSchedule.DayOfWeek &&
                ((newSchedule.StartTime >= existing.StartTime && newSchedule.StartTime < existing.EndTime) ||
                 (newSchedule.EndTime > existing.StartTime && newSchedule.EndTime <= existing.EndTime) ||
                 (newSchedule.StartTime <= existing.StartTime && newSchedule.EndTime >= existing.EndTime))
            );
        }
    }
}
