using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IGradeRepository
    {
        Task<List<StudentGrade>> GetByGroupAsync(Guid groupId);
        Task<List<StudentGrade>> GetByGroupAndSubjectAsync(Guid groupId, Guid subjectId);
        Task<StudentGrade?> GetByIdAsync(Guid gradeId);
        Task<StudentGrade?> GetByStudentLesson(Guid studentId, Guid lessonId);
        Task AddAsync(StudentGrade studentGrade);
        Task DeleteAsync(StudentGrade srudentGrade);
        Task SaveChangesAsync();
    }
}
