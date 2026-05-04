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
    public class ParticipationRepository : IParticipationRepository
    {
        private readonly AppDbContext _db;

        public ParticipationRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<LessonParticipation?> Get(Guid studentId, Guid lessonId)
        {
            return await _db.LessonParticipations
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.LessonId == lessonId);
        }

        public async Task AddAsync(LessonParticipation participation)
        {
            await _db.LessonParticipations.AddAsync(participation);
        }
    }
}
