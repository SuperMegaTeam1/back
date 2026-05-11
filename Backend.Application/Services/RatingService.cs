using Backend.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Entities;
using Backend.Application.Models.Rating;
using Backend.Application.Interfaces.Repository;

namespace Backend.Application.Services
{
    public sealed class RatingService : IRatingService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IGradeRepository _gradeRepo;
        private readonly ISubjectRepository _subjectRepo;
        private readonly IStudentRatingRepository _ratingRepo;
        public readonly IStudyGroupRepository _studyGroupRepo;
        private readonly ILessonRepository _lessonRepo;

        public RatingService(
        IStudentRepository studentRepo,
        IGradeRepository gradeRepo,
        ISubjectRepository subjectRepo,
        IStudentRatingRepository ratingRepo,
        IStudyGroupRepository studyGroupRepository,
        ILessonRepository lessonRepository)
        {
            _studentRepo = studentRepo;
            _gradeRepo = gradeRepo;
            _subjectRepo = subjectRepo;
            _ratingRepo = ratingRepo;
            _studyGroupRepo = studyGroupRepository;
            _lessonRepo = lessonRepository;
        }
        public async Task<StudentRatingResponse> GetMyRatingAsync(Guid userId, Guid? subjectId)
        {
            Console.WriteLine(userId);
            var student = await _studentRepo.GetByUserIdAsync(userId)
                ?? throw new Exception("Student not found");

            List<StudentRating> ratings;

            if (subjectId != null)
            {
                ratings = await _ratingRepo.GetByGroupAndSubjectAsync(
                    student.StudyGroupId,
                    subjectId.Value);
            }
            else
            {
                ratings = await _ratingRepo.GetByGroupAsync(student.StudyGroupId);
            }

            var me = ratings.FirstOrDefault(x => x.StudentId == student.Id);

            return new StudentRatingResponse
            {
                GroupId = student.StudyGroupId,
                GroupName = student.StudyGroup!.Name,

                SubjectId = subjectId,

                RatingPosition = me?.RatingPosition ?? 0,

                TotalGrade = me?.TotalGrade ?? 0,

                UpdatedAt = DateTime.UtcNow,

                TopStudents = ratings.Select(x => new TopStudentDto
                {
                    StudentId = x.StudentId,
                    FirstName = x.Student.FirstName,
                    LastName = x.Student.LastName,
                    FatherName = x.Student.FatherName,
                    TotalGrade = x.TotalGrade,
                    RatingPosition = x.RatingPosition
                }).ToList()
            };
        }

        public async Task UpdateRatingsAsync()
        {
            var result = new List<StudentRating>();

            var groups = await _studyGroupRepo.GetAllAsync();

            foreach (var group in groups)
            {
                var lessons = await _lessonRepo.GetLessonsByStudyGroup(group.Id);

                if (!lessons.Any())
                    continue;

                // Общий рейтинг по группе (по всем предметам)
                var groupGrades = await _gradeRepo.GetByGroupAsync(group.Id);

                var groupRatings = groupGrades
                    .GroupBy(g => g.StudentId)
                    .Select(g => new
                    {
                        StudentId = g.Key,
                        Avg = g.Average(x => x.Grade)
                    })
                    .OrderByDescending(x => x.Avg)
                    .Select((x, index) => new StudentRating
                    {
                        Id = Guid.NewGuid(),
                        StudentId = x.StudentId,
                        GroupId = group.Id,
                        SubjectId = null,
                        TotalGrade = x.Avg,
                        RatingPosition = index + 1,
                        UpdatedAt = DateTime.UtcNow
                    })
                    .ToList();

                result.AddRange(groupRatings);

                // Рейтинг по предметам
                var subjectIds = lessons
                    .Where(l => l?.SubjectId != null)
                    .Select(l => l!.SubjectId)
                    .Distinct();

                foreach (var subjectId in subjectIds)
                {
                    var subjectGrades = await _gradeRepo.GetByGroupAndSubjectAsync(group.Id, subjectId);

                    var subjectRatings = subjectGrades
                        .GroupBy(g => g.StudentId)
                        .Select(g => new
                        {
                            StudentId = g.Key,
                            Avg = g.Average(x => x.Grade)
                        })
                        .OrderByDescending(x => x.Avg)
                        .Select((x, index) => new StudentRating
                        {
                            Id = Guid.NewGuid(),
                            StudentId = x.StudentId,
                            GroupId = group.Id,
                            SubjectId = subjectId,
                            TotalGrade = x.Avg,
                            RatingPosition = index + 1,
                            UpdatedAt = DateTime.UtcNow
                        })
                        .ToList();

                    result.AddRange(subjectRatings);
                }
            }

            await _ratingRepo.ClearAsync();
            await _ratingRepo.AddRangeAsync(result);
        }
    }
}
