using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.Journal;

namespace Backend.Application.Interfaces
{
    public interface ILessonRepository
    {
        Task<Lesson?> GetByIdAsync(Guid id);

        Task<IReadOnlyList<Lesson?>> GetLessonsByStudyGroup(Guid studyGroupId);
        Task<IReadOnlyList<Lesson?>> GetLessonsByStudyGroupAndSubject(Guid studyGroupId, Guid subjectId);
        Task<IReadOnlyList<Lesson?>> GetLessonsByTeacherSubjectAndStudyGroup(Guid subjectId, Guid studyGroupId, Guid teacherUserId);
        Task SaveChangesAsync();

        Task<List<JournalInfoDto>> GetListDateSubjectAndGradeBySubject(Guid studentId, Guid studyGroupId, Guid subjectId);
    }
}
