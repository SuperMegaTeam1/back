using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.Subject;

namespace Backend.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _db;

        public SubjectRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<SubjectDto?> GetSubjectByIdAsync(Guid subjectId)
        {
            return await _db.Subjects
                .Where(subject => subject.Id == subjectId)
                .Select(subject => new SubjectDto(
                    subject.Id,
                    subject.Name,
                    subject.TeacherId))
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetNameByIdAsync(Guid id)
        {
            return await _db.Subjects
                .Where(x => x.Id == id)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<SubjectEntity>> GetSubjectsByStudyGroupIdAsync(Guid studyGroupId)
        {
            return await _db.Lessons
                .Where(l => l.StudyGroupId == studyGroupId)
                .Select(l => l.Subject)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<SubjectEntity>> GetSubjectsByTeacherIdAsync(Guid userId)
        {
            var teacherId = await _db.Teachers
                .Where(t => t.ParentUserId == userId)
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

            return await _db.Lessons
                .Where(l => l.TeacherId == teacherId)
                .Select(l => l.Subject)
                .Distinct()
                .ToListAsync();
        }
    }
}
