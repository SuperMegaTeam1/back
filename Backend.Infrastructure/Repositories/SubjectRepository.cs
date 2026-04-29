using Backend.Application.Interfaces;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _db;

        public SubjectRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<string?> GetNameByIdAsync(Guid id)
        {
            return await _db.Subjects
                .Where(x => x.Id == id)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();
        }
    }
}
