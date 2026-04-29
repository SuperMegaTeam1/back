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
    }
}
