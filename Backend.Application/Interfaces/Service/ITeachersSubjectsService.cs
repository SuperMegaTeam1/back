using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.StudentsSubjects;

namespace Backend.Application.Interfaces.Service
{
    public interface ITeachersSubjectsService
    {
        Task<IReadOnlyList<SubjectsResponse>> GetSubjectsForTeacherAsync(Guid userId);
    }
}
