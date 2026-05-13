using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Backend.Application.Models.Journal;

namespace Backend.Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _db;

        public LessonRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Lesson?> GetByIdAsync(Guid id)
        {
            return await _db.Lessons.FindAsync(id);
        }

        public async Task<IReadOnlyList<Lesson?>> GetLessonsByStudyGroup(Guid studyGroupId)
        {
            return await _db.Lessons
               .Where(l => l.StudyGroupId == studyGroupId)
               .ToListAsync();
        }

        public async Task<IReadOnlyList<Lesson?>> GetLessonsByStudyGroupAndSubject(Guid studyGroupId, Guid subjectId)
        {
            return await _db.Lessons
               .Where(l => l.StudyGroupId == studyGroupId && l.SubjectId == subjectId)
               .ToListAsync();
        }

        public async Task<IReadOnlyList<Lesson?>> GetLessonsByTeacherSubjectAndStudyGroup(Guid subjectId, Guid studyGroupId, Guid userId)
        {
            var teacherId = await _db.Teachers
               .Where(t => t.ParentUserId == userId)
               .Select(t => t.Id)
               .FirstOrDefaultAsync();

            return await _db.Lessons
                .Include(l => l.Grades)
                .Include(l => l.Participations)
                .Where(l => l.SubjectId == subjectId && l.StudyGroupId == studyGroupId && l.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
        
        public async Task<List<JournalInfoDto>> GetListDateSubjectAndGradeBySubject(Guid studentId, Guid studyGroupId, Guid subjectId)
        {
            return await  _db.Lessons
                .Where(lessons => lessons.StudyGroupId == studyGroupId
                                  && lessons.SubjectId == subjectId)
                .Join(
                    _db.StudentGrades.Where(studentGrade => studentGrade.StudentId == studentId),
                    lessons => lessons.Id,
                    studentGrades => studentGrades.LessonId,
                    (lessons, studentGrades) => new JournalInfoDto(
                        lessons.StartsAt,
                        studentGrades.Grade
                        ))
                .ToListAsync();
        }
    }
}
