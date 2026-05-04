using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IParticipationRepository
    {
        Task<LessonParticipation?> Get(Guid studentId, Guid lessonId);
        Task AddAsync(LessonParticipation participation);

        Task SaveChangesAsync();
    }
}
