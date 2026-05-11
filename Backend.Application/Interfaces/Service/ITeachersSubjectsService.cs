using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.Subjects;

namespace Backend.Application.Interfaces.Service
{
    public interface ITeachersSubjectsService
    {
        Task<IReadOnlyList<TeachersSubjectsResponse>> GetSubjectsForTeacherAsync(Guid userId);
    }
}
