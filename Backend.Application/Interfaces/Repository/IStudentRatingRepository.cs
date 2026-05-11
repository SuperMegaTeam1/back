using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces.Repository
{
    public interface IStudentRatingRepository
    {
        Task ClearAsync();

        Task AddRangeAsync(List<StudentRating> ratings);

        Task<List<StudentRating>> GetByGroupAsync(Guid groupId);

        Task<List<StudentRating>> GetByGroupAndSubjectAsync(
            Guid groupId,
            Guid subjectId);
    }
}
