using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly AppDbContext _db;

        public GradeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<StudentGrade>> GetByGroupAsync(Guid groupId)
        {
            return await _db.StudentGrades
                .Where(g => g.Student.StudyGroupId == groupId)
                .Include(g => g.Student)
                .ToListAsync();
        }

        public async Task<List<StudentGrade>> GetByGroupAndSubjectAsync(Guid groupId, Guid subjectId)
        {
            return await _db.StudentGrades
                .Where(g => g.Student.StudyGroupId == groupId &&
                            g.Lesson.SubjectId == subjectId)
                .Include(g => g.Student)
                .ToListAsync();
        }

        public async Task<StudentGrade?> GetByIdAsync(Guid gradeId)
        {
            return await _db.StudentGrades
                .Include(x => x.Lesson)
                    .ThenInclude(l => l.Subject)
                .FirstOrDefaultAsync(x => x.Id == gradeId);
        }

        public async Task<StudentGrade?> GetByStudentLesson(Guid studentId, Guid lessonId)
        {
            return await _db.StudentGrades
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.LessonId == lessonId);
        }

        public async Task AddAsync(StudentGrade grade)
        {
            await _db.StudentGrades.AddAsync(grade);
        }

        public async Task DeleteAsync(StudentGrade studentGrade)
        {
            await _db.StudentGrades.Where(s => s.Id == studentGrade.Id).ExecuteDeleteAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
