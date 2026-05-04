using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{

    public interface IStudentRepository
    {
        Task<Student?> GetByUserIdAsync(Guid userId);
        Task<List<Student>> GetByGroupIdAsync(Guid groupId);
    }
}