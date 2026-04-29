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
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _db;

        public StudentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Student?> GetByUserIdAsync(Guid userId)
        {
            return await _db.Students
                .Include(s => s.StudyGroup)
                .FirstOrDefaultAsync(s => s.ParentUserId == userId);
        }

        public async Task<List<Student>> GetByGroupIdAsync(Guid groupId)
        {
            return await _db.Students
                .Where(s => s.StudyGroupId == groupId)
                .ToListAsync();
        }
    }
}
