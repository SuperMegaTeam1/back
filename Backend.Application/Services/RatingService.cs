using Backend.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Entities;
using Backend.Application.Models.Rating;

namespace Backend.Application.Services
{
    public sealed class RatingService : IRatingService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IGradeRepository _gradeRepo;
        private readonly ISubjectRepository _subjectRepo;

        public RatingService(
        IStudentRepository studentRepo,
        IGradeRepository gradeRepo,
        ISubjectRepository subjectRepo)
        {
            _studentRepo = studentRepo;
            _gradeRepo = gradeRepo;
            _subjectRepo = subjectRepo;
        }

        public async Task<StudentRatingResponse> GetMyRatingAsync(Guid userId, Guid? subjectId)
        {
            var student = await _studentRepo.GetByUserIdAsync(userId)
                ?? throw new Exception("Student not found");
            var studentId = student.Id;
            var groupId = student.StudyGroupId;
            var studentsInGroup = await _studentRepo.GetByGroupIdAsync(groupId);
            var studentsDict = studentsInGroup.ToDictionary(x => x.Id);



            Console.WriteLine(student.StudyGroupId);
            Console.WriteLine(student.StudyGroup == null);

            List<StudentGrade> grades;

            if (subjectId != null)
            {
                grades = await _gradeRepo.GetByGroupAndSubjectAsync(student.StudyGroupId, subjectId.Value);
            }
            else
            {
                grades = await _gradeRepo.GetByGroupAsync(student.StudyGroupId);
            }

            if (grades == null || grades.Count == 0)
            {
                return new StudentRatingResponse
                {
                    GroupId = groupId,
                    GroupName = student.StudyGroup!.Name,
                    SubjectId = subjectId,
                    SubjectName = null,
                    RatingPosition = 0,
                    TotalGrade = 0,
                    UpdatedAt = DateTime.UtcNow,
                    TopStudents = new List<TopStudentDto>()
                };
            }

            var ratings = grades
                .GroupBy(g => g.StudentId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    AvgGrade = g.Average(x => x.Grade)
                })
                .OrderByDescending(x => x.AvgGrade)
                .ToList();

            var ranked = ratings
                .Select((x, index) => new
                {
                    x.StudentId,
                    x.AvgGrade,
                    Position = index + 1
                })
                .ToList();

            var me = ranked.First(x => x.StudentId == student.Id);

            var top = ranked.Take(5).ToList();

            string? subjectName = null;

            if (me == null)
            {
                return new StudentRatingResponse
                {
                    GroupId = groupId,
                    GroupName = student.StudyGroup!.Name,
                    SubjectId = subjectId,
                    SubjectName = subjectName,
                    RatingPosition = 0,
                    TotalGrade = 0,
                    UpdatedAt = DateTime.UtcNow,
                    TopStudents = top.Select(t => new TopStudentDto
                    {
                        StudentId = t.StudentId,
                        FirstName = studentsDict[t.StudentId].FirstName,
                        LastName = studentsDict[t.StudentId].LastName,
                        FatherName = studentsDict[t.StudentId].FatherName,
                        TotalGrade = t.AvgGrade,
                        RatingPosition = t.Position
                    }).ToList()
                };
            }

            if (subjectId != null)
                subjectName = await _subjectRepo.GetNameByIdAsync(subjectId.Value);

            return new StudentRatingResponse
            {
                GroupId = groupId,
                GroupName = student.StudyGroup!.Name,

                SubjectId = subjectId,
                SubjectName = subjectName,

                RatingPosition = me.Position,
                TotalGrade = me.AvgGrade,

                UpdatedAt = DateTime.UtcNow,

                TopStudents = top
                .Where(t => studentsDict.ContainsKey(t.StudentId))
                .Select(t =>
                {
                    var student = studentsDict[t.StudentId];

                    return new TopStudentDto
                    {
                        StudentId = t.StudentId,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        FatherName = student.FatherName,
                        TotalGrade = t.AvgGrade,
                        RatingPosition = t.Position
                    };
                }).ToList()
            };
        }
    }
}
