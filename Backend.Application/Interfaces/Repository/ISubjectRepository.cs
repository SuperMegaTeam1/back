using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.Subject;

namespace Backend.Application.Interfaces
{
    public interface ISubjectRepository
    {
        Task<SubjectDto> GetSubjectByIdAsync(Guid subjectId);
        
        Task<string?> GetNameByIdAsync(Guid subjectId);

        Task<IReadOnlyList<SubjectEntity>> GetSubjectsByStudyGroupIdAsync(Guid studyGroupId);

        Task<IReadOnlyList<SubjectEntity>> GetSubjectsByTeacherIdAsync(Guid teacherId);
    }
}
